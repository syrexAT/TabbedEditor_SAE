using System.Collections;
using System.Collections.Generic;

namespace SimpleWorld.ToolDevelopment_SAE.Data
{
	public class TileData //wichtig das es eine klasse ist und kein struct!
	{
		public TileType TileType; //pure C#, nur eine Klasse, hat nichts mit Unity zu tun, heisst aber nicht das wir sie nicht benutzen k�nnen in Unity!
		public int EnemyCount; //pro zelle wie viel enemeis da sein k�nnen, will dann hinzuf�gen und wegnehemen k�nnen im editor
	}
}