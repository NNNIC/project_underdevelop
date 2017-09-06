using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorUtil  {
    public static Color Mod_R(Color v, float r) { return new Color(  r, v.g, v.b, v.a); }
    public static Color Mod_G(Color v, float g) { return new Color(v.r,   g, v.b, v.a); }
    public static Color Mod_B(Color v, float b) { return new Color(v.r, v.g,   b, v.a); }
    public static Color Mod_A(Color v, float a) { return new Color(v.r, v.g, v.b,   a); }
}
