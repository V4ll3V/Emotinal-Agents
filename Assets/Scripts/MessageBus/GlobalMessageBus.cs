using UnityEngine;
using System.Collections;

namespace MessageBus
{
    public class GlobalMessageBus
    {
        private static MessageBus _instance;

        public static MessageBus Instance
        {
            get { return _instance ?? (_instance = new MessageBus()); }
        }
    }
}
