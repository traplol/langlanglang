#include <stddef.h>
#include <stdio.h>
#include <stdint.h>

void lll_Main(void);
void func_foo_3(int8_t var_n_4);
void func_foo_5(int16_t var_n_6);
void func_foo_7(int32_t var_n_8);
void func_foo_9(int64_t var_n_10);
void func_foo_11(uint8_t var_n_12);
void func_foo_13(uint16_t var_n_14);
void func_foo_15(uint32_t var_n_16);
void func_foo_17(uint64_t var_n_18);
int main(int argc, char** argv);


void lll_Main(void)
{
    int32_t __tmp27 = 8;
    int8_t var_x8_19 = __tmp27;
    int32_t __tmp28 = 16;
    int16_t var_x16_20 = __tmp28;
    int32_t __tmp29 = 32;
    int32_t var_x32_21 = __tmp29;
    int32_t __tmp30 = 64;
    int64_t var_x64_22 = __tmp30;
    int32_t __tmp31 = 8;
    uint8_t var_ux8_23 = __tmp31;
    int32_t __tmp32 = 16;
    uint16_t var_ux16_24 = __tmp32;
    int32_t __tmp33 = 36;
    uint32_t var_ux32_25 = __tmp33;
    int32_t __tmp34 = 64;
    uint64_t var_ux64_26 = __tmp34;
    func_foo_3(var_x8_19);
    func_foo_5(var_x16_20);
    func_foo_7(var_x32_21);
    func_foo_9(var_x64_22);
    func_foo_11(var_ux8_23);
    func_foo_13(var_ux16_24);
    func_foo_15(var_ux32_25);
    func_foo_17(var_ux64_26);
    int32_t __tmp35 = 100;
    func_foo_3(__tmp35);
    int32_t __tmp36 = 10000;
    func_foo_5(__tmp36);
    int32_t __tmp37 = 1000000000;
    func_foo_7(__tmp37);
    int64_t __tmp38 = 10000000000000000;
    func_foo_9(__tmp38);
}
void func_foo_3(int8_t var_n_4)
{
    char* __tmp39 = "i8: %d\n";
    int32_t __tmp40 = printf(__tmp39, var_n_4);
}
void func_foo_5(int16_t var_n_6)
{
    char* __tmp41 = "i16: %d\n";
    int32_t __tmp42 = printf(__tmp41, var_n_6);
}
void func_foo_7(int32_t var_n_8)
{
    char* __tmp43 = "i32: %d\n";
    int32_t __tmp44 = printf(__tmp43, var_n_8);
}
void func_foo_9(int64_t var_n_10)
{
    char* __tmp45 = "i64: %lld\n";
    int32_t __tmp46 = printf(__tmp45, var_n_10);
}
void func_foo_11(uint8_t var_n_12)
{
    char* __tmp47 = "u8: %d\n";
    int32_t __tmp48 = printf(__tmp47, var_n_12);
}
void func_foo_13(uint16_t var_n_14)
{
    char* __tmp49 = "u16: %d\n";
    int32_t __tmp50 = printf(__tmp49, var_n_14);
}
void func_foo_15(uint32_t var_n_16)
{
    char* __tmp51 = "u32: %d\n";
    int32_t __tmp52 = printf(__tmp51, var_n_16);
}
void func_foo_17(uint64_t var_n_18)
{
    char* __tmp53 = "u64: %d\n";
    int32_t __tmp54 = printf(__tmp53, var_n_18);
}
int main(int argc, char** argv)
{
    lll_Main();
    int32_t __tmp55 = 0;
    return __tmp55;
}

