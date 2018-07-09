using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Oxide.Plugins.JtechCore.Util
{
    static class ContenersOffset
    {
        public static Vector3 Turret = new Vector3(0, -0.58f, 0);
        public static Vector3 Refinery = new Vector3(-1, 0, -0.1f);
        public static Vector3 Furnace = new Vector3(0, -0.3f, 0);
        public static Vector3 Largefurnace = new Vector3(0, -1.5f, 0);
        public static Vector3 Searchlight = new Vector3(0, -0.5f, 0);
        public static Vector3 Pumpfuel = Vector3.zero;
        public static Vector3 Pumpoutput = new Vector3(-1, 2, 0);
        public static Vector3 Eecycler = Vector3.zero;
        public static Vector3 Largewatercatcher = new Vector3(0, -0.6f, 0);
        public static Vector3 Smallwatercatcher = new Vector3(0, -0.6f, 0);
        public static Vector3 Waterbarrel = new Vector3(0, 0.2f, 0);
        public static Vector3 Waterpurifier = new Vector3(0, 0.25f, 0);
        public static Vector3 Bbq = Vector3.up * 0.03f;
        //public static Vector3 quarryfuel = new Vector3(1,-0.2f,0);
        //public static Vector3 quarryoutput = new Vector3(1,0,0);
    }
}
