#include <stddef.h>
#include <stdio.h>
#include <stdint.h>
struct struc_OStream_0;

void lll_Main(void);
void ext_OStream_Ctor_4(struct struc_OStream_0* var_this_5);
struct struc_OStream_0* ext_OStream___lshift___6(struct struc_OStream_0* var_this_7, char* var_val_8);
struct struc_OStream_0* ext_OStream___lshift___9(struct struc_OStream_0* var_this_10, int32_t var_val_11);
int main(int argc, char** argv);

struct struc_OStream_0
{
    int32_t x;
};

void lll_Main(void)
{
    uint64_t __tmp17 = sizeof(struct struc_OStream_0);
    void* __tmp18 = malloc(__tmp17);
    struct struc_OStream_0* var___tmp13_14 = __tmp18;
    ext_OStream_Ctor_4(var___tmp13_14);
    struct struc_OStream_0* var_out_12 = var___tmp13_14;
    char* __tmp19 = "\n";
    char* var_endl_15 = __tmp19;
    {
        int32_t __tmp20 = 0;
        int8_t var_i_16 = __tmp20;
L21:
        {
            int32_t __tmp22 = 100;
            int __tmp23 = var_i_16 <= __tmp22;
            if (__tmp23)
            {
                struct struc_OStream_0* __tmp24 = ext_OStream___lshift___9(var_out_12, var_i_16);
                char* __tmp25 = "hello world";
                struct struc_OStream_0* __tmp26 = ext_OStream___lshift___6(__tmp24, __tmp25);
                int32_t __tmp27 = 42;
                struct struc_OStream_0* __tmp28 = ext_OStream___lshift___9(__tmp26, __tmp27);
                int32_t __tmp29 = 123;
                int32_t __tmp30 = (-(__tmp29));
                struct struc_OStream_0* __tmp31 = ext_OStream___lshift___9(__tmp28, __tmp30);
                char* __tmp32 = "boop";
                struct struc_OStream_0* __tmp33 = ext_OStream___lshift___6(__tmp31, __tmp32);
                struct struc_OStream_0* __tmp34 = ext_OStream___lshift___6(__tmp33, var_endl_15);
                int32_t __tmp35 = 1;
                int32_t __tmp36 = var_i_16 + __tmp35;
                var_i_16 = __tmp36;
                goto L21;
            }
        }
    }
}
void ext_OStream_Ctor_4(struct struc_OStream_0* var_this_5)
{
}
struct struc_OStream_0* ext_OStream___lshift___6(struct struc_OStream_0* var_this_7, char* var_val_8)
{
    char* __tmp37 = "%s ";
    int32_t __tmp38 = printf(__tmp37, var_val_8);
    return var_this_7;
}
struct struc_OStream_0* ext_OStream___lshift___9(struct struc_OStream_0* var_this_10, int32_t var_val_11)
{
    char* __tmp39 = "%ld ";
    int32_t __tmp40 = printf(__tmp39, var_val_11);
    return var_this_10;
}
int main(int argc, char** argv)
{
    lll_Main();
    int32_t __tmp41 = 0;
    return __tmp41;
}

