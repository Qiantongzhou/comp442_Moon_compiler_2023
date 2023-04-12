         org   0
 message db    "Hello, world!", 13, 10, 0
 message1 db    "fuckyou", 0,0,0
         org   30
         align
         entry              % Start here
         add   r2,r0,r0
 pri     lb    r3,message1(r2)   % Get next char
         ceqi  r4,r3,0
         putc   r3
         addi   r2,r2,1
         j      pri             % Go for next char


