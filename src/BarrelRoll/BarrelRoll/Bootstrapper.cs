using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.DependencyInjection;
using UnityEngine.Events;
using UnityEngine.Logging;
using UnityEngine.UI;

namespace BarrelRoll
{
    public class Bootstrapper : MonoBehaviour
    {
        public const bool DefaultRecordConfigurations = false;

        private ILogger _logger;
        private Configurator _configurator;

        private const string MSG_LOGGERS =
            nameof(BootLoggerProvider) + " is used for logging during the boot phase, when services are being registered and configurations loaded. " +
            nameof(RuntimeLoggerProvider) + " is used for logging while game systems are actually running.";

        [Tooltip(MSG_LOGGERS)]
        public DebugLoggerProvider BootLoggerProvider;

        [Tooltip(MSG_LOGGERS)]
        public DebugLoggerProvider RuntimeLoggerProvider;

        [Header("Configuration")]
        public ConfigurationSourceCollection ConfigurationSourceCollection;

        [Tooltip(
            "This flag toggles the encapsulated " + nameof(Configurator) + "'s " + nameof(Configurator.RecordingConfigurations) + " property. " +
            "Use this to toggle whether configurations are being recorded at runtime, to help with optimization."
        )]
        public bool RecordingConfigurations = DefaultRecordConfigurations;
        public UnityEvent ConfigLoadingComplete = new();

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        private void Awake()
        {
            this.AssertAssociation(BootLoggerProvider, nameof(BootLoggerProvider));
            this.AssertAssociation(RuntimeLoggerProvider, nameof(RuntimeLoggerProvider));
            this.AssertAssociation(ConfigurationSourceCollection, nameof(ConfigurationSourceCollection));

            _logger = BootLoggerProvider.GetLogger(this);

            foreach (Type clientType in new[] {
                typeof(Configurable),
                typeof(Updatable),
                typeof(AudioMixerParameterSlider),
            }) {
                DependencyInjector.Instance.CacheResolution(clientType);
            }

            _configurator = new Configurator(BootLoggerProvider) { RecordingConfigurations = RecordingConfigurations };
            foreach (Type clientType in Array.Empty<Type>()) {
                _configurator.CacheConfiguration(clientType, Configurable.DefaultConfigKey(clientType));
            }
            _configurator.LoadingComplete += (sender, configs) => {
                _logger.Log($"All {nameof(ConfigurationSource)}s loaded");
                ConfigLoadingComplete.Invoke();
            };

            var runtimIdProvider = new RuntimeIdProvider();

            DependencyInjector.Instance.Initialize(BootLoggerProvider);
            DependencyInjector.Instance.RegisterService<ILoggerProvider>(RuntimeLoggerProvider);
            DependencyInjector.Instance.RegisterService<IConfigurator>(_configurator);
            DependencyInjector.Instance.RegisterService<IRuntimeIdProvider>(runtimIdProvider);

            StartCoroutine(_configurator.LoadConfigsAsync(ConfigurationSourceCollection.ConfigurationSources));
        }

        [Conditional("UNITY_EDITOR")]
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Unity message")]
        private void Reset()
        {
            BootLoggerProvider = null;
            RuntimeLoggerProvider = null;
            ConfigurationSourceCollection = null;

            RecordingConfigurations = DefaultRecordConfigurations;
        }

    }
}
