	entry
	addi r1,r0,topaddr
	addi r14,r0,topaddr
	subi r1,r1,0
	addi r13,r0,64
	sw t3(r0),r13
	addi r12, r0, 4
	lw r13,t3(r0)
	sw arr(r12),r13
	addi r12,r0,34
	sw t4(r0),r12
	addi r12, r0, 8
	lw r13,t4(r0)
	sw arr(r12),r13
	addi r12,r0,25
	sw t5(r0),r12
	addi r12, r0, 12
	lw r13,t5(r0)
	sw arr(r12),r13
	addi r12,r0,12
	sw t6(r0),r12
	addi r12, r0, 16
	lw r13,t6(r0)
	sw arr(r12),r13
	addi r12,r0,22
	sw t7(r0),r12
	addi r12, r0, 20
	lw r13,t7(r0)
	sw arr(r12),r13
	addi r12,r0,11
	sw t8(r0),r12
	addi r12, r0, 24
	lw r13,t8(r0)
	sw arr(r12),r13
	addi r12,r0,90
	sw t9(r0),r12
	addi r12, r0, 28
	lw r13,t9(r0)
	sw arr(r12),r13
	addi r14,r14,0
	jl r15,printarray0
	subi r14,r14,0
printarray0	sw -4(r14),r15
	addi r12, r0, 8
	lw r10,arr(r12)
	sw -8(r14), r10
	addi r1,r0,buf
	sw -12(r14),r1
	jl r15, intstr
	sw -8(r14),r13
	jl r15,putstr
	lw r15,-4(r14)
	jr r15
	hlt
arr1 res 28
t3 res 4
t4 res 4
t5 res 4
t6 res 4
t7 res 4
t8 res 4
t9 res 4
buf res 20

putstr    lw    r1,-8(r14)    % i := r1
          addi  r2,r0,0
putstr1   lb    r2,0(r1)      % ch := B[i]
          ceqi  r3,r2,0
          bnz   r3,putstr2    % branch if ch = 0
          putc  r2
          addi  r1,r1,1       % i++
          j     putstr1
putstr2   jr    r15
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

