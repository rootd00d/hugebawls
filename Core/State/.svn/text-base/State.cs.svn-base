
namespace HugeBawls.State
{
    public abstract class State<EntityType>
    {
        public abstract void Enter(EntityType e);
        public abstract void Execute(EntityType e);
        public abstract void Exit(EntityType e);

        public virtual bool OnMessage(EntityType e, Message m)
        {
            UI.LogMessage("Message received");
            return true;
        }


    }

    
}