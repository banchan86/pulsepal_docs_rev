﻿using System;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Bonsai.IO.Ports;

namespace Bonsai.PulsePal
{
    /// <summary>
    /// Represents an operator that configures output channel parameters on a
    /// Pulse Pal device.
    /// </summary>
    [Description("Configures output channel parameters on a Pulse Pal device.")]
    public class ConfigureOutputChannel : Sink
    {
        const string ChannelCategory = "Channel";
        const string VoltageCategory = "Pulse Voltage";
        const string TimingCategory = "Pulse Timing";
        const string CustomTrainCategory = "CustomTrain";
        const string TriggerCategory = "Pulse Trigger";
        const double MinVoltage = -10;
        const double MaxVoltage = 10;
        const int VoltageDecimalPlaces = 3;
        const double VoltageIncrement = 0.001;
        const double MinTimePeriod = 0.0001;
        const double MaxTimePeriod = 3600;
        const int TimeDecimalPlaces = 4;

        /// <summary>
        /// Gets or sets the name of the serial port used to communicate with the
        /// Pulse Pal device.
        /// </summary>
        [Category(ChannelCategory)]
        [TypeConverter(typeof(SerialPortNameConverter))]
        [Description("The name of the serial port used to communicate with the Pulse Pal device.")]
        public string PortName { get; set; }

        /// <summary>
        /// Gets or sets the output channel to configure.
        /// </summary>
        [Category(ChannelCategory)]
        [Description("The output channel to configure.")]
        public OutputChannel Channel { get; set; } = OutputChannel.Channel1;

        /// <summary>
        /// Gets or sets a value specifying whether to use biphasic or
        /// monophasic pulses.
        /// </summary>
        [Category(VoltageCategory)]
        [Description("Specifies whether to use biphasic or monophasic pulses.")]
        public bool Biphasic { get; set; }

        /// <summary>
        /// Gets or sets the voltage for the first phase of each pulse.
        /// </summary>
        [Category(VoltageCategory)]
        [Range(MinVoltage, MaxVoltage)]
        [Precision(VoltageDecimalPlaces, VoltageIncrement)]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Description("The voltage for the first phase of each pulse.")]
        public double Phase1Voltage { get; set; }

        /// <summary>
        /// Gets or sets the voltage for the second phase of each pulse.
        /// </summary>
        [Category(VoltageCategory)]
        [Range(MinVoltage, MaxVoltage)]
        [Precision(VoltageDecimalPlaces, VoltageIncrement)]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Description("The voltage for the second phase of each pulse.")]
        public double Phase2Voltage { get; set; }

        /// <summary>
        /// Gets or sets the resting voltage, in the range [-10, 10] volts.
        /// </summary>
        [Category(VoltageCategory)]
        [Range(MinVoltage, MaxVoltage)]
        [Precision(VoltageDecimalPlaces, VoltageIncrement)]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Description("The resting voltage.")]
        public double RestingVoltage { get; set; }

        /// <summary>
        /// Gets or sets the duration of the first phase of the pulse, in the range
        /// [0.0001, 3600] seconds.
        /// </summary>
        [Category(TimingCategory)]
        [Range(MinTimePeriod, MaxTimePeriod)]
        [Precision(TimeDecimalPlaces, MinTimePeriod)]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Description("The duration of the first phase of the pulse, in seconds.")]
        public double Phase1Duration { get; set; }

        /// <summary>
        /// Gets or sets the interval between the first and second phase of a biphasic pulse,
        /// in the range [0.0001, 3600] seconds.
        /// </summary>
        [Category(TimingCategory)]
        [Range(MinTimePeriod, MaxTimePeriod)]
        [Precision(TimeDecimalPlaces, MinTimePeriod)]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Description("The interval between the first and second phase of a biphasic pulse, in seconds.")]
        public double InterPhaseInterval { get; set; }

        /// <summary>
        /// Gets or sets the duration of the second phase of the pulse, in the range
        /// [0.0001, 3600] seconds.
        /// </summary>
        [Category(TimingCategory)]
        [Range(MinTimePeriod, MaxTimePeriod)]
        [Precision(TimeDecimalPlaces, MinTimePeriod)]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Description("The duration of the second phase of the pulse, in seconds.")]
        public double Phase2Duration { get; set; }

        /// <summary>
        /// Gets or sets the interval between pulses, in the range [0.0001, 3600] seconds.
        /// </summary>
        [Category(TimingCategory)]
        [Range(MinTimePeriod, MaxTimePeriod)]
        [Precision(TimeDecimalPlaces, MinTimePeriod)]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Description("The interval between pulses, in seconds.")]
        public double InterPulseInterval { get; set; }

        /// <summary>
        /// Gets or sets the duration of a pulse burst, in the range
        /// [0.0001, 3600] seconds.
        /// </summary>
        [Category(TimingCategory)]
        [Range(MinTimePeriod, MaxTimePeriod)]
        [Precision(TimeDecimalPlaces, MinTimePeriod)]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Description("The duration of a pulse burst, in seconds.")]
        public double BurstDuration { get; set; }

        /// <summary>
        /// Gets or sets the duration of the off-time between bursts, in the range
        /// [0.0001, 3600] seconds.
        /// </summary>
        [Category(TimingCategory)]
        [Range(MinTimePeriod, MaxTimePeriod)]
        [Precision(TimeDecimalPlaces, MinTimePeriod)]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Description("The duration of the off-time between bursts, in seconds.")]
        public double InterBurstInterval { get; set; }

        /// <summary>
        /// Gets or sets the duration of the pulse train, in the range
        /// [0.0001, 3600] seconds.
        /// </summary>
        [Category(TimingCategory)]
        [Range(MinTimePeriod, MaxTimePeriod)]
        [Precision(TimeDecimalPlaces, MinTimePeriod)]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Description("The duration of the pulse train, in seconds.")]
        public double PulseTrainDuration { get; set; }

        /// <summary>
        /// Gets or sets the delay to start the pulse train, in the range
        /// [0.0001, 3600] seconds.
        /// </summary>
        [Category(TimingCategory)]
        [Range(MinTimePeriod, MaxTimePeriod)]
        [Precision(TimeDecimalPlaces, MinTimePeriod)]
        [Editor(DesignTypes.SliderEditor, DesignTypes.UITypeEditor)]
        [Description("The delay to start the pulse train, in seconds.")]
        public double PulseTrainDelay { get; set; }

        /// <summary>
        /// Gets or sets a value specifying the identity of the custom pulse train
        /// to use on this output channel.
        /// </summary>
        [Category(CustomTrainCategory)]
        [Description("Specifies the identity of the custom pulse train to use on this output channel.")]
        public CustomTrainId CustomTrainIdentity { get; set; }

        /// <summary>
        /// Gets or sets a value specifying the interpretation of pulse times in the
        /// custom pulse train.
        /// </summary>
        [Category(CustomTrainCategory)]
        [Description("Specifies the interpretation of pulse times in the custom pulse train.")]
        public CustomTrainTarget CustomTrainTarget { get; set; }

        /// <summary>
        /// Gets or sets a value specifying whether the output channel
        /// will loop its custom pulse train.
        /// </summary>
        [Category(CustomTrainCategory)]
        [Description("Specifies whether the output channel will loop its custom pulse train.")]
        public bool CustomTrainLoop { get; set; }

        /// <summary>
        /// Gets or sets a value specifying whether trigger channel 1 can trigger
        /// this output channel.
        /// </summary>
        [Category(TriggerCategory)]
        [Description("Specifies whether trigger channel 1 can trigger this output channel.")]
        public bool TriggerOnChannel1 { get; set; }

        /// <summary>
        /// Gets or sets a value specifying whether trigger channel 2 can trigger
        /// this output channel.
        /// </summary>
        [Category(TriggerCategory)]
        [Description("Specifies whether trigger channel 2 can trigger this output channel.")]
        public bool TriggerOnChannel2 { get; set; }

        /// <summary>
        /// Configures the output channel parameters on the Pulse Pal device whenever
        /// an observable sequence emits a notification.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used to trigger configuration
        /// of the Pulse Pal output channel.
        /// </param>
        /// <returns>
        /// An observable sequence that is identical to the <paramref name="source"/>
        /// sequence but where there is an additional side effect of configuring the
        /// output channel parameters on the Pulse Pal device whenever the sequence
        /// emits a notification.
        /// </returns>
        public override IObservable<TSource> Process<TSource>(IObservable<TSource> source)
        {
            return Observable.Using(
                cancellationToken => PulsePalManager.ReserveConnectionAsync(PortName),
                (connection, cancellationToken) => Task.FromResult(source.Do(input =>
                {
                    lock (connection.PulsePal)
                    {
                        var channel = Channel;
                        connection.PulsePal.SetBiphasic(channel, Biphasic);
                        connection.PulsePal.SetPhase1Voltage(channel, Phase1Voltage);
                        connection.PulsePal.SetPhase2Voltage(channel, Phase2Voltage);
                        connection.PulsePal.SetPhase1Duration(channel, Phase1Duration);
                        connection.PulsePal.SetInterPhaseInterval(channel, InterPhaseInterval);
                        connection.PulsePal.SetPhase2Duration(channel, Phase2Duration);
                        connection.PulsePal.SetInterPulseInterval(channel, InterPulseInterval);
                        connection.PulsePal.SetBurstDuration(channel, BurstDuration);
                        connection.PulsePal.SetInterBurstInterval(channel, InterBurstInterval);
                        connection.PulsePal.SetPulseTrainDuration(channel, PulseTrainDuration);
                        connection.PulsePal.SetPulseTrainDelay(channel, PulseTrainDelay);
                        connection.PulsePal.SetTriggerOnChannel1(channel, TriggerOnChannel1);
                        connection.PulsePal.SetTriggerOnChannel2(channel, TriggerOnChannel2);
                        connection.PulsePal.SetCustomTrainIdentity(channel, CustomTrainIdentity);
                        connection.PulsePal.SetCustomTrainTarget(channel, CustomTrainTarget);
                        connection.PulsePal.SetCustomTrainLoop(channel, CustomTrainLoop);
                        connection.PulsePal.SetRestingVoltage(channel, RestingVoltage);
                    }
                })));
        }
    }
}