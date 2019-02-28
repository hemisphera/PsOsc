﻿using eos.Mvvm.Core;
using Rug.Osc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hsp.PsOsc
{

  public class OscInterface : ViewModelBase, IDisposable
  {

    private OscReceiver Receiver { get; set; }

    private OscSender Sender { get; set; }

    public bool Connected
    {
      get => GetAutoFieldValue<bool>();
      private set => SetAutoFieldValue(value);
    }

    public string ReceiverState
    {
      get => GetAutoFieldValue<string>();
      private set => SetAutoFieldValue(value);
    }

    public string SenderState
    {
      get => GetAutoFieldValue<string>();
      private set => SetAutoFieldValue(value);
    }

    public List<MessageHandlerBase> Handlers { get; }

    private CancellationTokenSource Token { get; }


    public OscInterface()
    {
      Handlers = new List<MessageHandlerBase>();
      Token = new CancellationTokenSource();

      Task.Run(() =>
      {
        while (true)
        {
          ReceiverState = $"{Receiver?.State ?? OscSocketState.Closed}";
          SenderState = $"{Sender?.State ?? OscSocketState.Closed}";
          Thread.Sleep(500);
          Token.Token.ThrowIfCancellationRequested();
        }
      });
    }


    private void OscReceiverTaskHandler()
    {
      try
      {
        while (Receiver.State == OscSocketState.Connected)
        {
          var packet = Receiver.Receive();
          ProcessMessage(packet);
        }
      }
      catch
      {
        // ignore
      }
    }

    private void ProcessMessage(OscPacket packet)
    {
      var messages = packet is OscBundle bundle ? bundle.Cast<OscMessage>().ToArray() : new[] { packet as OscMessage };

      foreach (var message in messages)
      foreach (var handler in Handlers)
      {
        var m = handler.Regex.Match(message.Address);
        if (!m.Success) continue;

        var groupNames = handler.Regex.GetGroupNames();
        var args = groupNames.ToDictionary(
          name => name,
          name => m.Groups[name].Value);
        handler.Process(args, message.ToArray());
      }
    }

    public void Send(string address, params object[] arguments)
    {
      if (Sender == null)
        throw new InvalidOperationException("The OSC interface is not connected.");

      Sender.Send(new OscMessage(address, arguments));
    }


    public void Connect()
    {
      Receiver = new OscReceiver(IPAddress.Any, Properties.Settings.Default.LocalPort);
      Receiver.Connect();

      Sender = new OscSender(IPAddress.Parse(Properties.Settings.Default.DawHostname), 0, Properties.Settings.Default.DawPort);
      Sender.Connect();

      Task.Run(() => OscReceiverTaskHandler());

      Send("/device/region/count", 0);
      Send("/device/region/count", Engine.RegionCount);
      Send("/device/track/count", 0);
      Send("/device/track/count", Engine.TrackCount);

      Send("/action", 41743); // refresh surfaces

      Connected = true;
    }

    public void Disconnect()
    {
      Sender?.Close();
      Sender?.Dispose();
      Sender = null;

      Receiver?.Close();
      Receiver?.Dispose();
      Receiver = null;

      Connected = false;
    }


    public void Dispose()
    {
      Token.Cancel();
      Disconnect();
    }

  }

}