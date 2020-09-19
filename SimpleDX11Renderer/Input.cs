using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Reflection;

namespace SimpleDX11Renderer
{
    internal struct KeySubscriber
    {
        public Key TargetKey;
        public MethodInfo SourceMethod;
        public object SourceClassInstaqnce;

        public KeySubscriber (Key targetKey, MethodInfo sourceMethod, object sourceClassInstaqnce)
        {
            TargetKey = targetKey;
            SourceMethod = sourceMethod;
            SourceClassInstaqnce = sourceClassInstaqnce;
        }
    }

    public static class Input
    {
        private static List<KeySubscriber> keySubscriber = new List<KeySubscriber>();

        public static void SubscribeToKey(Key TargetKey, MethodInfo SourceMethod, object SourceClassInstaqnce)
        {
            keySubscriber.Add(new KeySubscriber(TargetKey, SourceMethod, SourceClassInstaqnce));
           
        }

        public static void CallKeySubscribers()
        {
            for (int i = 0; i < keySubscriber.Count; i++)
            {
                try
                {
                    if(Keyboard.IsKeyDown(keySubscriber[i].TargetKey))
                    {
                        keySubscriber[i].SourceMethod.Invoke(keySubscriber[i].SourceClassInstaqnce, new object[] { keySubscriber[i].TargetKey });
                    }
                }

                catch
                {
                    Debug.Error("Can not invoke method for Key subscriber: " + keySubscriber[i].SourceMethod.Name + " of the key: " + keySubscriber[i].TargetKey);
                }
            }
        }
    }
}