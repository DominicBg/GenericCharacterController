[System.Serializable]
public struct BitFlag
{
    static int numberBitsInInt = 32;
    public int[] bitflag;

    void Init()
    {
        bitflag = new int[5];
    }

    public void Set(int id, bool value)
    {
        if (bitflag == null || bitflag.Length == 0)
            Init();

        int level = id / numberBitsInInt;
        int bitID = (1 << id);

        if (value) //set a 1
            bitflag[level] = bitflag[level] | bitID;
        else //set a 0
        {
            if (Get(id))
                bitflag[level] -= bitID;
        }
    }
    public bool Get(int id)
    {
        if (bitflag == null || bitflag.Length == 0)
            return false;

        int level = id / numberBitsInInt;

        int bitMask = 1 << id;
        return ((bitflag[level] & bitMask) == bitMask);
    }
}