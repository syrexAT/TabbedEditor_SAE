using System.Collections;
using System.Collections.Generic;

namespace SimpleWorld.ToolDevelopment_SAE.Data //Company name . Project name . Folder in dem ers hineinspeichert
{
	public enum TileType
	{
		//wir geben ihm werte, weil enum is einfach nur int im hintergrund, normalerweise ist es 0,1,2,3 zugewisen, aber man kann es h�ndisch eingeben
		//warum aber? --> wenn man was einf�gen will, z.B. wenns ein ablauf is eine sinnvolle ordnung, dann kann ich was einf�gen dazwischen und einfach z.B. 150 geben.
		//Vorteil: beim Dropdown im inspector! wenn man hier das �ndert z.B. was hinzuf�gt mit 150 dann 
		//bei uns hier eigentlich egal
		Grass = 100,
		Stone = 200,
		Sand = 300,
		Water = 400
	}
}