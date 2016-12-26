namespace SecurityAccess
{
    public interface SecurityAccess
    {
        byte[] seedToKey(uint mask, byte[] seed);
    }
}
