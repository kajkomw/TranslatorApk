﻿using System;
using TranslatorApkPluginLib;

using static TranslatorApk.Logic.OrganisationItems.Utils;

namespace TranslatorApk.Logic.Classes
{
    public class TransServiceHost : MarshalByRefObject, ITranslateService
    {
        private readonly string _serviceName;
        private readonly Func<string, string, string, string> _translate;

        public TransServiceHost(object innerClass)
        {
            Type type = innerClass.GetType();

            _serviceName = ExecRefl<string>(type, innerClass, "GetServiceName");
            Guid = ExecRefl<Guid>(type, innerClass, "get_Guid");

            _translate = CreateDelegate<Func<string, string, string, string>>(innerClass, "Translate");
        }

        public string GetServiceName() => _serviceName;

        public string Translate(string text, string targetLanguage, string apiKey)
        {
            return _translate(text, targetLanguage, apiKey);
        }

        public Guid Guid { get; }

        public override object InitializeLifetimeService() => null;
    }
}
