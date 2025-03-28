namespace Shouldly.Tests.Strings.DetailedDifference.CaseSensitive.LongStrings.MultipleDiffs;

public class LongRunOfDiffsAreConsolidatedAndContinued
{
    [Fact]
    public void LongRunOfDiffsAreConsolidatedAndContinuedShouldFail()
    {
        var str = "1a,1b,1c,1d,1e,1f,1g,1h,1i,1j,1k,1l,1m,1n,1o,1p,1q,1r,1s,1t,1u,1v";
        Verify.ShouldFail(() =>
                str.ShouldBe("2A.2B.2C.2D.2E.2F.2G.2H.2I.2J.2K.2L.2M.2N.2O.2P.2Q.2R.2S.2T.2U.2V"),

            errorWithSource:
            """
            str
                should be
            "2A.2B.2C.2D.2E.2F.2G.2H.2I.2J.2K.2L.2M.2N.2O.2P.2Q.2R.2S.2T.2U.2V"
                but was
            "1a,1b,1c,1d,1e,1f,1g,1h,1i,1j,1k,1l,1m,1n,1o,1p,1q,1r,1s,1t,1u,1v"
                difference
            Difference     |  |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |        
                           | \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/       
            Index          | 0    1    2    3    4    5    6    7    8    9    10   11   12   13   14   15   16   17   18   19   20   ...  
            Expected Value | 2    A    .    2    B    .    2    C    .    2    D    .    2    E    .    2    F    .    2    G    .    ...  
            Actual Value   | 1    a    ,    1    b    ,    1    c    ,    1    d    ,    1    e    ,    1    f    ,    1    g    ,    ...  
            Expected Code  | 50   65   46   50   66   46   50   67   46   50   68   46   50   69   46   50   70   46   50   71   46   ...  
            Actual Code    | 49   97   44   49   98   44   49   99   44   49   100  44   49   101  44   49   102  44   49   103  44   ...  

            Difference     |       |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |        
                           |      \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/       
            Index          | ...  21   22   23   24   25   26   27   28   29   30   31   32   33   34   35   36   37   38   39   40   41   ...  
            Expected Value | ...  2    H    .    2    I    .    2    J    .    2    K    .    2    L    .    2    M    .    2    N    .    ...  
            Actual Value   | ...  1    h    ,    1    i    ,    1    j    ,    1    k    ,    1    l    ,    1    m    ,    1    n    ,    ...  
            Expected Code  | ...  50   72   46   50   73   46   50   74   46   50   75   46   50   76   46   50   77   46   50   78   46   ...  
            Actual Code    | ...  49   104  44   49   105  44   49   106  44   49   107  44   49   108  44   49   109  44   49   110  44   ...  

            Difference     |       |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |        
                           |      \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/       
            Index          | ...  42   43   44   45   46   47   48   49   50   51   52   53   54   55   56   57   58   59   60   61   62   ...  
            Expected Value | ...  2    O    .    2    P    .    2    Q    .    2    R    .    2    S    .    2    T    .    2    U    .    ...  
            Actual Value   | ...  1    o    ,    1    p    ,    1    q    ,    1    r    ,    1    s    ,    1    t    ,    1    u    ,    ...  
            Expected Code  | ...  50   79   46   50   80   46   50   81   46   50   82   46   50   83   46   50   84   46   50   85   46   ...  
            Actual Code    | ...  49   111  44   49   112  44   49   113  44   49   114  44   49   115  44   49   116  44   49   117  44   ...  

            Difference     |       |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |   
                           |      \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  
            Index          | ...  44   45   46   47   48   49   50   51   52   53   54   55   56   57   58   59   60   61   62   63   64   
            Expected Value | ...  .    2    P    .    2    Q    .    2    R    .    2    S    .    2    T    .    2    U    .    2    V    
            Actual Value   | ...  ,    1    p    ,    1    q    ,    1    r    ,    1    s    ,    1    t    ,    1    u    ,    1    v    
            Expected Code  | ...  46   50   80   46   50   81   46   50   82   46   50   83   46   50   84   46   50   85   46   50   86   
            Actual Code    | ...  44   49   112  44   49   113  44   49   114  44   49   115  44   49   116  44   49   117  44   49   118  
            """,

            errorWithoutSource:
            """
            "1a,1b,1c,1d,1e,1f,1g,1h,1i,1j,1k,1l,1m,1n,1o,1p,1q,1r,1s,1t,1u,1v"
                should be
            "2A.2B.2C.2D.2E.2F.2G.2H.2I.2J.2K.2L.2M.2N.2O.2P.2Q.2R.2S.2T.2U.2V"
                but was not
                difference
            Difference     |  |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |        
                           | \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/       
            Index          | 0    1    2    3    4    5    6    7    8    9    10   11   12   13   14   15   16   17   18   19   20   ...  
            Expected Value | 2    A    .    2    B    .    2    C    .    2    D    .    2    E    .    2    F    .    2    G    .    ...  
            Actual Value   | 1    a    ,    1    b    ,    1    c    ,    1    d    ,    1    e    ,    1    f    ,    1    g    ,    ...  
            Expected Code  | 50   65   46   50   66   46   50   67   46   50   68   46   50   69   46   50   70   46   50   71   46   ...  
            Actual Code    | 49   97   44   49   98   44   49   99   44   49   100  44   49   101  44   49   102  44   49   103  44   ...  

            Difference     |       |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |        
                           |      \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/       
            Index          | ...  21   22   23   24   25   26   27   28   29   30   31   32   33   34   35   36   37   38   39   40   41   ...  
            Expected Value | ...  2    H    .    2    I    .    2    J    .    2    K    .    2    L    .    2    M    .    2    N    .    ...  
            Actual Value   | ...  1    h    ,    1    i    ,    1    j    ,    1    k    ,    1    l    ,    1    m    ,    1    n    ,    ...  
            Expected Code  | ...  50   72   46   50   73   46   50   74   46   50   75   46   50   76   46   50   77   46   50   78   46   ...  
            Actual Code    | ...  49   104  44   49   105  44   49   106  44   49   107  44   49   108  44   49   109  44   49   110  44   ...  

            Difference     |       |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |        
                           |      \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/       
            Index          | ...  42   43   44   45   46   47   48   49   50   51   52   53   54   55   56   57   58   59   60   61   62   ...  
            Expected Value | ...  2    O    .    2    P    .    2    Q    .    2    R    .    2    S    .    2    T    .    2    U    .    ...  
            Actual Value   | ...  1    o    ,    1    p    ,    1    q    ,    1    r    ,    1    s    ,    1    t    ,    1    u    ,    ...  
            Expected Code  | ...  50   79   46   50   80   46   50   81   46   50   82   46   50   83   46   50   84   46   50   85   46   ...  
            Actual Code    | ...  49   111  44   49   112  44   49   113  44   49   114  44   49   115  44   49   116  44   49   117  44   ...  

            Difference     |       |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |    |   
                           |      \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  \|/  
            Index          | ...  44   45   46   47   48   49   50   51   52   53   54   55   56   57   58   59   60   61   62   63   64   
            Expected Value | ...  .    2    P    .    2    Q    .    2    R    .    2    S    .    2    T    .    2    U    .    2    V    
            Actual Value   | ...  ,    1    p    ,    1    q    ,    1    r    ,    1    s    ,    1    t    ,    1    u    ,    1    v    
            Expected Code  | ...  46   50   80   46   50   81   46   50   82   46   50   83   46   50   84   46   50   85   46   50   86   
            Actual Code    | ...  44   49   112  44   49   113  44   49   114  44   49   115  44   49   116  44   49   117  44   49   118  
            """);
    }

    [Fact]
    public void ShouldPass()
    {
        "1a,1b,1c,1d,1e,1f,1g,1h,1i,1j,1k,1l,1m,1n,1o,1p,1q,1r,1s,1t,1u,1v"
            .ShouldBe("1a,1b,1c,1d,1e,1f,1g,1h,1i,1j,1k,1l,1m,1n,1o,1p,1q,1r,1s,1t,1u,1v");
    }
}