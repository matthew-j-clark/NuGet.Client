// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Common;

namespace NuGet.Protocol
{
    internal class EnhancedHttpRetryHelper
    {
        /// <summary>
        /// The default delay in milliseconds between retries.
        /// </summary>
        public const int DefaultDelayMilliseconds = 1000;

        /// <summary>
        /// The default value indicating whether or not enhanced HTTP retry is enabled.
        /// </summary>
        public const bool DefaultEnabled = true;

        /// <summary>
        /// The default number of times to retry.
        /// </summary>
        public const int DefaultRetryCount = 6;

        /// <summary>
        /// The environment variable used to change the delay value.
        /// </summary>
        public const string ExperimentalRetryDelayMsEnvVarName = "NUGET_EXPERIMENTAL_NETWORK_RETRY_DELAY_MILLISECONDS";

        /// <summary>
        /// The environment variable used to enable or disable the enhanced HTTP retry..
        /// </summary>
        public const string ExperimentalRetryEnabledEnvVarName = "NUGET_ENABLE_EXPERIMENTAL_HTTP_RETRY";

        /// <summary>
        /// The environment variable used to change the retry value.
        /// </summary>
        public const string ExperimentalRetryTryCountEnvVarName = "NUGET_EXPERIMENTAL_MAX_NETWORK_TRY_COUNT";

        private readonly IEnvironmentVariableReader _environmentVariableReader;

        private bool? _enhancedHttpRetryIsEnabled = null;

        private int? _experimentalMaxNetworkTryCountValue = null;

        private int? _experimentalRetryDelayMillisecondsValue = null;

        public EnhancedHttpRetryHelper(IEnvironmentVariableReader environmentVariableReader)
        {
            _environmentVariableReader = environmentVariableReader ?? throw new ArgumentNullException(nameof(environmentVariableReader));
        }

        /// <summary>
        /// Gets a value indicating whether or not enhanced HTTP retry should be enabled.  The default value is <c>true</c>.
        /// </summary>
        internal bool EnhancedHttpRetryEnabled => _enhancedHttpRetryIsEnabled ??= GetBoolFromEnvironmentVariable(ExperimentalRetryEnabledEnvVarName, defaultValue: DefaultEnabled, _environmentVariableReader);

        /// <summary>
        /// Gets a value indicating the maximum number of times to retry.  The default value is 6.
        /// </summary>
        internal int ExperimentalMaxNetworkTryCount => _experimentalMaxNetworkTryCountValue ??= GetIntFromEnvironmentVariable(ExperimentalRetryTryCountEnvVarName, defaultValue: DefaultRetryCount, _environmentVariableReader);

        /// <summary>
        /// Gets a value indicating the delay in milliseconds to wait before retrying a connection.  The default value is 1000.
        /// </summary>
        internal int ExperimentalRetryDelayMilliseconds => _experimentalRetryDelayMillisecondsValue ??= GetIntFromEnvironmentVariable(ExperimentalRetryDelayMsEnvVarName, defaultValue: DefaultDelayMilliseconds, _environmentVariableReader);

        /// <summary>
        /// Gets a <see cref="bool" /> value from the specified environment variable.
        /// </summary>
        /// <param name="variableName">The name of the environment variable to get the value.</param>
        /// <param name="defaultValue">The default value to return if the environment variable is not defined or is not a valid <see cref="bool" />.</param>
        /// <param name="environmentVariableReader">An <see cref="IEnvironmentVariableReader" /> to use when reading the environment variable.</param>
        /// <returns>The value of the specified as a <see cref="bool" /> if the specified environment variable is defined and is a valid value for <see cref="bool" />.</returns>
        private static bool GetBoolFromEnvironmentVariable(string variableName, bool defaultValue, IEnvironmentVariableReader environmentVariableReader)
        {
            try
            {
                if (bool.TryParse(environmentVariableReader.GetEnvironmentVariable(variableName), out bool parsedValue))
                {
                    return parsedValue;
                }
            }
            catch (Exception) { }

            return defaultValue;
        }

        /// <summary>
        /// Gets an <see cref="int" /> value from the specified environment variable.
        /// </summary>
        /// <param name="variableName">The name of the environment variable to get the value.</param>
        /// <param name="defaultValue">The default value to return if the environment variable is not defined or is not a valid <see cref="int" />.</param>
        /// <param name="environmentVariableReader">An <see cref="IEnvironmentVariableReader" /> to use when reading the environment variable.</param>
        /// <returns>The value of the specified as a <see cref="int" /> if the specified environment variable is defined and is a valid value for <see cref="int" />.</returns>
        private static int GetIntFromEnvironmentVariable(string variableName, int defaultValue, IEnvironmentVariableReader environmentVariableReader)
        {
            try
            {
                if (int.TryParse(environmentVariableReader.GetEnvironmentVariable(variableName), out int parsedValue))
                {
                    return parsedValue;
                }
            }
            catch (Exception) { }

            return defaultValue;
        }
    }
}
