    entry
    align
    addi r1,r0,topaddr
    addi r14,r0,topaddr
    addi r1,r1,-12
    addi r3,r0,7
    sw  -8(r14),r3
    addi r4,r0,57
    sw -4(r14),r4
    lw r5, -8(r14)
    lw r6, -4(r14)
    add  r5,r5,r6
    sw -8(r14),r5
    addi r1,r0,buf
    sw -12(r14),r1
    jl r15, intstr
    sw -8(r14),r13
    jl r15,putstr


    hlt
buf res 12
                           % All done!
% cd source\repos\442-a2\A5_output_moon

putstr    lw    r1,-8(r14)    % i := r1
          addi  r2,r0,0
putstr1   lb    r2,0(r1)      % ch := B[i]
          ceqi  r3,r2,0
          bnz   r3,putstr2    % branch if ch = 0
          putc  r2
          addi  r1,r1,1       % i++
          j     putstr1
putstr2   jr    r15

putint	align
	add	r2,r0,r0		% Initialize buffers index i
	cge	r3,r1,r0		% True if N >= 0
	bnz	r3,putint1		% Branch if True (N >= 0)
	sub	r1,r0,r1		% N = -N
putint1	modi	r4,r1,10		% Rightmost digit
	addi	r4,r4,48		% Convert to ch
	divi	r1,r1,10		% Truncate rightmost digit
	sb	putint9(r2),r4		% Store ch in buffer
	addi	r2,r2,1			% i++
	bnz	r1,putint1		% Loop if not finished
	bnz	r3,putint2		% Branch if True (N >= 0)
	addi	r3,r0,45
	sb	putint9(r2),r3		% Store '-' in buffer
	addi	r2,r2,1			% i++
	add	r1,r0,r0		% Initialize output register (r1 = 0)
putint2	subi	r2,r2,1			% i--
	lb	r1,putint9(r2)		% Load ch from buffer
	putc	r1			% Output ch
	bnz	r2,putint2		% Loop if not finished
	jr	r15			% return to the caller
putint9	res	12			% loacl buffer (12 bytes)
	align

intstr    lw    r13,-12(r14)
          addi  r13,r13,11    % r13 points to end of buffer
          sb    0(r13),r0     % store terminator
          lw    r1,-8(r14)    % r1 := N (to be converted)
          addi  r2,r0,0       % S := 0 (sign)
          cgei  r3,r1,0
          bnz   r3,intstr1    % branch if N >= 0
          addi  r2,r2,1       % S := 1
          sub   r1,r0,r1      % N := -N
intstr1   addi  r3,r1,0       % D := N (next digit)
          modi  r3,r3,10      % D mod= 10
          addi  r3,r3,48      % D += "0"
          subi  r13,r13,1     % i--
          sb    0(r13),r3     % B[i] := D
          divi  r1,r1,10      % N div= 10
          cnei  r3,r1,0
          bnz   r3,intstr1    % branch if N != 0
          ceqi  r3,r2,0
          bnz   r3,intstr2    % branch if S = 0
          subi  r13,r13,1     % i--
          addi  r3,r0,45
          sb    0(r13),r3     % B[i] := "-"
intstr2   jr    r15
