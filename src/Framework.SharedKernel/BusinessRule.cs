namespace Framework.SharedKernel;

public abstract record BusinessRule
{
    public abstract string Message { get; }

    public override string ToString() => Message;
}
