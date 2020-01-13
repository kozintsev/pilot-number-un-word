using System;
using System.Collections.Generic;
using Ascon.Pilot.SDK.ObjectCard;
using System.ComponentModel.Composition;
using System.Globalization;
using Ascon.Pilot.SDK.NumberInWord.Model;
using Newtonsoft.Json;
using NumberInWords;

namespace Ascon.Pilot.SDK.NumberInWord
{
    [Export(typeof(IObjectCardHandler))]
    [Export(typeof(ISettingsFeature))]
    public class NumberInWordConverter : IObjectCardHandler, ISettingsFeature
    {
        private List<TripleNumberTextAttr> _tripleNumberTextAttr;

        [ImportingConstructor]
        public NumberInWordConverter(IPersonalSettings personalSettings)
        {
            Key = "NumberInWordConverter-E4D4E10E-F4AE-40A1-AD9A-FB50A3FA8485";
            Title = "Конвертер денежных величин";
            Editor = null;
            LoadSettings(personalSettings);
        }

        public bool Handle(IAttributeModifier modifier, ObjectCardContext context)
        {
            var isObjectModification = context.EditiedObject != null;
            if (context.IsReadOnly)
                return false;

            if (_tripleNumberTextAttr == null)
            {
                return false;
            }

            foreach (var item in _tripleNumberTextAttr)
            {
                if (context.AttributesValues.TryGetValue(item.NumberAttr, out var attr))
                {
                    if (attr is decimal n)
                    {
                        var st = n.ToString(CultureInfo.CurrentCulture);
                        var kop = string.Empty;
                        var split = st.Split('.');
                        if (split.Length == 2)
                        {
                            kop = split[1];
                        }
                        var split1 = st.Split(',');
                        if (split1.Length == 2)
                        {
                            kop = split1[1];
                        }

                        var t = Math.Truncate(n);
                        var j = decimal.ToInt32(t);

                        var s = RusNumber.Str(j).Trim();

                        if (!string.IsNullOrEmpty(kop))
                        {
                            s = s + " " + kop + " копеек";
                        }

                        var s2 = n.ToString("N");

                        var pos = s2.LastIndexOf(',');
                        var substring = s2.Substring(0, pos);

                        modifier.SetValue(item.StrAttr, s);
                        modifier.SetValue(item.StrNumberAttr, substring + " рублей " + kop + " копеек");
                    }
                }
            }

            return true;
        }

        public bool OnValueChanged(IAttribute sender, AttributeValueChangedEventArgs args, IAttributeModifier modifier)
        {
            return false;
        }

        public void SetValueProvider(ISettingValueProvider settingValueProvider)
        {
            
        }

        public string Key { get; }
        public string Title { get; }
        public System.Windows.FrameworkElement Editor { get; }

        private async void LoadSettings(IPersonalSettings personalSettings)
        {
            var setting = new SettingsLoader(personalSettings);
            var json = await setting.Load();
            _tripleNumberTextAttr = GetListTripleNumberTextAttr(json);
        }

        private static List<TripleNumberTextAttr> GetListTripleNumberTextAttr(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<List<TripleNumberTextAttr>>(json);

            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
