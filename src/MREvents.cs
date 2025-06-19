namespace MRCustom;

public static partial class MREvents
{
    //
    // APPLY/REMOVE HOOKS
    //

    // Add hooks
    internal static void ApplyEvents()
    {
        On.Creature.Grab += Creature_Grab;
        On.Creature.ReleaseGrasp += Creature_ReleaseGrasp;
        On.Creature.SwitchGrasps += Creature_SwitchGrasp;

        On.Player.Collide += Player_Collide;
    }

    // Add hooks
    internal static void RemoveEvents()
    {
        On.Creature.Grab -= Creature_Grab;
        On.Creature.ReleaseGrasp -= Creature_ReleaseGrasp;
        On.Creature.SwitchGrasps -= Creature_SwitchGrasp;

        On.Player.Collide -= Player_Collide;
    }

    //
    // EVENT HOOKAGE
    //

    //
    // CREATURES
    //

    private static bool Creature_Grab(On.Creature.orig_Grab orig, Creature self, PhysicalObject grabbedObj, int graspUsed, int chunkGrabbed, Creature.Grasp.Shareability shareability, float dominance, bool overrideEquallyDominant, bool pacifying)
    {
        bool result = orig(self, grabbedObj, graspUsed, chunkGrabbed, shareability, dominance, overrideEquallyDominant, pacifying);

        OnCreatureGrab?.Invoke(self, grabbedObj, graspUsed, chunkGrabbed, shareability, dominance, overrideEquallyDominant, pacifying);
        if (self is Player)
            OnPlayerGrab?.Invoke((Player)self, grabbedObj, graspUsed, chunkGrabbed, shareability, dominance, overrideEquallyDominant, pacifying);

        return result;
    }

    private static void Creature_ReleaseGrasp(On.Creature.orig_ReleaseGrasp orig, Creature self, int grasp)
    {
        orig(self, grasp);

        OnCreatureReleaseGrasp?.Invoke(self, grasp);
        if (self is Player)
            OnPlayerReleaseGrasp?.Invoke((Player)self, grasp);
    }

    private static void Creature_SwitchGrasp(On.Creature.orig_SwitchGrasps orig, Creature self, int fromGrasp, int toGrasp)
    {
        orig(self, fromGrasp, toGrasp);

        OnCreatureSwitchGrasp?.Invoke(self, fromGrasp, toGrasp);
        if (self is Player)
            OnPlayerSwitchGrasp?.Invoke((Player)self, fromGrasp, toGrasp);
    }

    //
    // PLAYERS
    //

    private static void Player_Collide(On.Player.orig_Collide orig, Player self, PhysicalObject otherObject, int myChunk, int otherChunk)
    {
        orig(self, otherObject, myChunk, otherChunk);
            OnPlayerCollide?.Invoke(self, otherObject, myChunk, otherChunk);
    }

    //
    // CREATURE EVENTS
    //

    /// <summary>
    /// Event triggered when a player grabs an object.
    /// </summary>
    public static event Action<Creature, PhysicalObject, int, int, Creature.Grasp.Shareability, float, bool, bool> OnCreatureGrab;

    /// <summary>
    /// Event triggered when a player releases an object.
    /// </summary>
    public static event Action<Creature, int> OnCreatureReleaseGrasp;

    /// <summary>
    /// Event triggered when a creature switches held object to another grasp.
    /// </summary>
    public static event Action<Creature, int, int> OnCreatureSwitchGrasp;

    //
    // PLAYER EVENTS
    //

    /// <summary>
    /// Event triggered when a player grabs an object.
    /// </summary>
    public static event Action<Player, PhysicalObject, int, int, Creature.Grasp.Shareability, float, bool, bool> OnPlayerGrab;

    /// <summary>
    /// Event triggered when a player releases an object.
    /// </summary>
    public static event Action<Player, int> OnPlayerReleaseGrasp;

    /// <summary>
    /// Event triggered when a player switches held object to another grasp.
    /// </summary>
    public static event Action<Player, int, int> OnPlayerSwitchGrasp;

    /// <summary>
    /// Event triggered whenever the player collides.
    /// </summary>
    public static event Action<Player, PhysicalObject, int, int> OnPlayerCollide;
}