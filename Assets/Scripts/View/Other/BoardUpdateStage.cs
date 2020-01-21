
namespace Match3SampleView
{
    public enum BoardUpdateStage
    {
        AwaitingForUpdate = -1,
        HandleMove = 0,
        HandleMove_WaitForEnd,
        DestroyMatches,
        DestroyMatches_WaitForEnd,
        MovesOnBoard,
        MovesOnBoard_WaitForEnd,
        MovesFromQueuesToBoard,
        MovesFromQueuesToBoard_WaitForEnd,
        MovesInQueues,
        MovesInQueues_WaitForEnd,
        InstanceNewToQueues,
        InstanceNewToQueues_WaitForEnd
    }
}
