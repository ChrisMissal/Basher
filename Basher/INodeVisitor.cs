namespace Basher
{
    internal interface INodeVisitor
    {
        void Visit(ReplayElement element);
    }
}