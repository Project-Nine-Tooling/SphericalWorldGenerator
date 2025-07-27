using System;

namespace SphericalWorldGenerator.Gameplay
{
    // 1) Stub out MonoBehaviour if your classes inherit from it
    public class MonoBehaviour { }

    // 2) Minimal KeyCode enum (add more keys as you need them)
    public enum KeyCode
    {
        F1 = ConsoleKey.F1,
        F2 = ConsoleKey.F2,
        F3 = ConsoleKey.F3,
        F4 = ConsoleKey.F4,
        F5 = ConsoleKey.F5,
        // …etc.
    }

    // 3) Simple Input.GetKeyDown that maps to ConsoleKey
    public static class Input
    {
        public static bool GetKeyDown(KeyCode key)
        {
            if (!Console.KeyAvailable)
                return false;

            ConsoleKeyInfo info = Console.ReadKey(intercept: true);
            // note: ConsoleKey and our KeyCode share the same numeric values
            return info.Key == (ConsoleKey)key;
        }
    }
}
