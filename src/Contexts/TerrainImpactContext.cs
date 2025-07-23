using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRCustom.Contexts;

public class TerrainImpactContext
{
    public int chunkIndex;

    public IntVector2 direction;

    public float speed;

    public bool firstContact;

    public TerrainImpactContext(int chunkIndex, IntVector2 direction, float speed, bool firstContact)
    {
        this.chunkIndex = chunkIndex;
        this.direction = direction;
        this.speed = speed;
        this.firstContact = firstContact;
    }
}
