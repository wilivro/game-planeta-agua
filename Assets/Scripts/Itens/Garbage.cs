using UnityEngine;
using System.Collections;

public class Garbage : Item
{

	public static int metal = 1;
    public static int vidro = 2;
    public static int papel = 3;
    public static int plastico = 4;
    public static int organico = 5;

    public int type;

    public Garbage() {

    }

	public Garbage(string _name) {
		name = _name;
	}
}
