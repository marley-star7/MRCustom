namespace MRCustom.CosmeticSprites;

// Token: 0x020003F6 RID: 1014
public class LightningBolt : CosmeticSprite
{
    public enum LightningBoltType : byte
    {
        Temporary = 0,
        Permanent = 1,
    }

    public float life;
    protected float lastLife;

    public float lifeTime;

    public Vector2 from;
    public Vector2 target;

    public LightningBoltType type;

    public float width;
    public float length;

    public float intensity;
    public float lightningType;

    public float randomOffset;
    public float lightningParam;

    private bool light;

    public Color? setColor;

    // Token: 0x06002F43 RID: 12099 RVA: 0x00374525 File Offset: 0x00372725
    public LightningBolt(Vector2 from, Vector2 target, LightningBoltType type, float width)
    {
        this.from = from;
        this.target = target;
        this.type = type;
        this.width = width * 30f;
        this.Init();
    }

    // Token: 0x06002F44 RID: 12100 RVA: 0x00374558 File Offset: 0x00372758
    public void Init()
    {
        this.lastLife = -1000f;
        this.life = 1f;
        this.randomOffset = UnityEngine.Random.value;
        if (this.type == LightningBoltType.Temporary)
        {
            return;
        }
        if (this.type == LightningBoltType.Permanent)
        {
            this.intensity = 1f;
            return;
        }
        Custom.LogWarning(new string[]
        {
            "Unknown Lightning Type"
        });
    }

    // Token: 0x06002F45 RID: 12101 RVA: 0x003745B7 File Offset: 0x003727B7
    public override void AddToContainer(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, FContainer newContatiner)
    {
        if (newContatiner == null)
        {
            newContatiner = rCam.ReturnFContainer("Foreground");
        }
        base.AddToContainer(sLeaser, rCam, newContatiner);
    }

    // Token: 0x06002F46 RID: 12102 RVA: 0x003745D2 File Offset: 0x003727D2
    public override void ApplyPalette(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, RoomPalette palette)
    {

    }

    // Token: 0x06002F47 RID: 12103 RVA: 0x003745D4 File Offset: 0x003727D4
    public override void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {
        Vector2 a = this.target - camPos;
        Vector2 b = this.from - camPos;
        Vector2 vector = (a - b) * 0.5f + b;
        float num = Mathf.Lerp(this.width, this.width * Custom.LerpQuadEaseOut(0f, 1f, this.Length / 50f), Custom.LerpQuadEaseOut(1f, 0f, this.width));
        sLeaser.sprites[0].x = vector.x;
        sLeaser.sprites[0].y = vector.y;
        sLeaser.sprites[0].rotation = Custom.AimFromOneVectorToAnother(this.from, this.target);
        sLeaser.sprites[0].scaleY = this.Length * 1.03f;
        sLeaser.sprites[0].scaleX = num;
        sLeaser.sprites[0].alpha = this.randomOffset;
        sLeaser.sprites[1].rotation = Custom.AimFromOneVectorToAnother(this.from, this.target);
        sLeaser.sprites[1].x = vector.x;
        sLeaser.sprites[1].y = vector.y;
        sLeaser.sprites[1].scaleY = this.Length * (1f + this.intensity);
        sLeaser.sprites[1].scaleX = (num + this.Length * 0.3f) * (1f + this.intensity);
        sLeaser.sprites[1].alpha = ((this.light && this.width > 0f) ? this.Parameters.r : 0f);

        if (this.setColor == null)
        {
            sLeaser.sprites[0].color = Parameters;
            sLeaser.sprites[1].color = Custom.HSL2RGB(this.lightningType - 0.0001f, 1f, 0.6f);
        }
        else
        {
            sLeaser.sprites[0].color = this.setColor.Value;
            sLeaser.sprites[1].color = this.setColor.Value;
            setColor = null;
        }

        if (base.slatedForDeletetion || this.room != rCam.room)
        {
            sLeaser.CleanSpritesAndRemove();
        }
    }

    // Token: 0x06002F48 RID: 12104 RVA: 0x003747EC File Offset: 0x003729EC
    public override void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
    {
        sLeaser.sprites = new FSprite[2];
        sLeaser.sprites[0] = new FSprite("Futile_White", true);
        sLeaser.sprites[0].shader = rCam.room.game.rainWorld.Shaders["LightningBolt"];
        sLeaser.sprites[0].color = this.Parameters;
        sLeaser.sprites[0].x = this.pos.x;
        sLeaser.sprites[0].y = this.pos.y;
        sLeaser.sprites[0].scaleX = this.width;
        sLeaser.sprites[0].scaleY = this.length;
        sLeaser.sprites[1] = new FSprite("Futile_White", true);
        sLeaser.sprites[1].x = this.pos.x;
        sLeaser.sprites[1].y = this.pos.y;
        sLeaser.sprites[1].scale = 10f;
        sLeaser.sprites[1].shader = rCam.room.game.rainWorld.Shaders["LightSource"];
        this.AddToContainer(sLeaser, rCam, null);
    }

    // Token: 0x06002F49 RID: 12105 RVA: 0x0037494C File Offset: 0x00372B4C
    public override void Update(bool eu)
    {
        base.Update(eu);
        this.lastLife = this.life;
        if (this.type == LightningBoltType.Temporary)
        {
            this.life -= 1f / this.lifeTime;
            if (this.lastLife <= 0f)
            {
                this.Destroy();
            }
        }
    }

    // Token: 0x17000806 RID: 2054
    // (get) Token: 0x06002F4A RID: 12106 RVA: 0x003749A0 File Offset: 0x00372BA0
    public float Length
    {
        get
        {
            return Custom.Dist(this.from, this.target) * 0.075f;
        }
    }

    // Token: 0x17000807 RID: 2055
    // (get) Token: 0x06002F4B RID: 12107 RVA: 0x003749B9 File Offset: 0x00372BB9
    public Color Parameters
    {
        get
        {
            return new Color(Mathf.Clamp(this.life, 0f, 1f) * this.intensity, this.lightningParam, this.lightningType);
        }
    }

    // Token: 0x06002F4C RID: 12108 RVA: 0x003749E8 File Offset: 0x00372BE8
    public LightningBolt(Vector2 from, Vector2 target, LightningBoltType type, float width, float lifeTime) : this(from, target, type, width)
    {
        this.lifeTime = lifeTime * 30f;
    }

    // Token: 0x06002F4D RID: 12109 RVA: 0x00374A03 File Offset: 0x00372C03
    public LightningBolt(Vector2 from, Vector2 target, LightningBoltType type, float width, float lifeTime, float lightningParam, float lightningType) : this(from, target, type, width, lifeTime)
    {
        this.lightningParam = lightningParam;
        this.lightningType = lightningType;
    }

    // Token: 0x06002F4E RID: 12110 RVA: 0x00374A22 File Offset: 0x00372C22
    /*
    public LightningBolt(Vector2 from, Vector2 target, LightningBoltType type, float width, float lifeTime, float lightningParam, float lightningType, bool light) : this(from, target, type, width, lifeTime, lightningParam, lightningType)
    {
        this.light = light;
        //this.color = Custom.HSL2RGB(this.lightningType - 0.0001f, 1f, 0.6f);
    }
    */
}
