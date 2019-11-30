using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace luval.tccr.config
{
    public class ConfigManager
    {
        private static Dictionary<string, string> _public;
        private static Dictionary<string, string> _private;
        private static Dictionary<string, string> _data;

        public static Dictionary<string, string> Setting
        {
            get
            {
                if (_data == null)
                    Load();
                return _data;
            }
        }

        private static void Load()
        {
            LoadPublic();
            LoadPrivate();
            Merge();
        }

        private static void Merge()
        {
            if (!_private.Keys.Any())
                _data = _public;
            else
                _data = new Dictionary<string, string>(
                    _public.Select(i => new KeyValuePair<string, string>(i.Key, i.Value)).Union(_private.Select(p => new KeyValuePair<string, string>(p.Key, p.Value)))
                    );
        }

        private static void LoadPublic()
        {
            _public = DictionaryLoad("config.json");
        }

        private static void LoadPrivate()
        {
            _private = DictionaryLoad("config.private.json");
        }

        private static Dictionary<string, String> DictionaryLoad(string name)
        {
            return File.Exists(name) ?
                JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(name)) :
                new Dictionary<string, string>();
        }
    }
}
