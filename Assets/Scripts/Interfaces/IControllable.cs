namespace Assets.Scripts.Interfaces
{
    public interface IControllable
    {
        void LeftSideUp(Assets.Scripts.Enums.GameState state);

        void LeftSideDown(Assets.Scripts.Enums.GameState state);

        void LeftSideRight(Assets.Scripts.Enums.GameState state);

        void LeftSideLeft(Assets.Scripts.Enums.GameState state);

        void RightSideUp(Assets.Scripts.Enums.GameState state);

        void RightSideDown(Assets.Scripts.Enums.GameState state);

        void RightSideLeft(Assets.Scripts.Enums.GameState state);

        void RightSideRight(Assets.Scripts.Enums.GameState state);
    }
}