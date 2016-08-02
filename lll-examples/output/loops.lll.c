#include <stddef.h>
#include <stdio.h>
#include <stdint.h>

void lll_Main(void);
int main(int argc, char** argv);


void lll_Main(void)
{
    char* __tmp8 = "\n=== 0..10 ===\n";
    int32_t __tmp9 = printf(__tmp8);
    {
        int32_t __tmp10 = 0;
        int32_t var_i_3 = __tmp10;
L11:
        {
            int32_t __tmp12 = 10;
            int __tmp13 = var_i_3 <= __tmp12;
            if (__tmp13)
            {
                char* __tmp14 = "%d\n";
                int32_t __tmp15 = printf(__tmp14, var_i_3);
                int32_t __tmp16 = 1;
                int32_t __tmp17 = var_i_3 + __tmp16;
                var_i_3 = __tmp17;
                goto L11;
            }
        }
    }
    char* __tmp18 = "\n=== 10..0 ===\n";
    int32_t __tmp19 = printf(__tmp18);
    {
        int32_t __tmp20 = 10;
        int32_t var_i_4 = __tmp20;
L21:
        {
            int32_t __tmp22 = 0;
            int __tmp23 = var_i_4 >= __tmp22;
            if (__tmp23)
            {
                char* __tmp24 = "%d\n";
                int32_t __tmp25 = printf(__tmp24, var_i_4);
                int32_t __tmp26 = 1;
                int32_t __tmp27 = var_i_4 - __tmp26;
                var_i_4 = __tmp27;
                goto L21;
            }
        }
    }
    int32_t __tmp28 = 10;
    int32_t var_N_5 = __tmp28;
    char* __tmp29 = "\n=== 0..N ===\n";
    int32_t __tmp30 = printf(__tmp29);
    {
        int32_t __tmp31 = 0;
        int32_t var_i_6 = __tmp31;
L32:
        {
            int __tmp33 = var_i_6 <= var_N_5;
            if (__tmp33)
            {
                char* __tmp34 = "%d\n";
                int32_t __tmp35 = printf(__tmp34, var_i_6);
                int32_t __tmp36 = 1;
                int32_t __tmp37 = var_i_6 + __tmp36;
                var_i_6 = __tmp37;
                goto L32;
            }
        }
    }
    char* __tmp38 = "\n=== N..0 ===\n";
    int32_t __tmp39 = printf(__tmp38);
    {
        int32_t var_i_7 = var_N_5;
L40:
        {
            int32_t __tmp41 = 0;
            int __tmp42 = var_i_7 >= __tmp41;
            if (__tmp42)
            {
                char* __tmp43 = "%d\n";
                int32_t __tmp44 = printf(__tmp43, var_i_7);
                int32_t __tmp45 = 1;
                int32_t __tmp46 = var_i_7 - __tmp45;
                var_i_7 = __tmp46;
                goto L40;
            }
        }
    }
}
int main(int argc, char** argv)
{
    lll_Main();
    int32_t __tmp47 = 0;
    return __tmp47;
}

