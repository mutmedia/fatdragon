using Assets.Scripts.Enums;

namespace Assets.Scripts.Interfaces
{
    public interface IControllable
    {
        void MoveLeftSide(CommandType command, GameState state);

        void MoveRightSide(CommandType command, GameState state);
    }
}