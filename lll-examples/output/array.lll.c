#include <stddef.h>
#include <stdio.h>
#include <stdint.h>

void lll_Main(void);
int main(int argc, char** argv);


void lll_Main(void)
{
    int32_t __tmp8 = 5;
    int32_t var_n_3 = __tmp8;
    size_t __tmp9 = sizeof(int32_t);
    size_t __tmp10 = var_n_3 * __tmp9;
    void* __tmp11 = malloc(__tmp10);
    int32_t* __tmp5 = __tmp11;
    int32_t* var_x_4 = __tmp5;
    char* __tmp12 = "setting values\n";
    int32_t __tmp13 = printf(__tmp12);
    {
        int32_t __tmp14 = 0;
        int32_t var_i_6 = __tmp14;
L15:
        {
            int32_t __tmp16 = 1;
            int32_t __tmp17 = var_n_3 - __tmp16;
            int __tmp18 = var_i_6 <= __tmp17;
            if (__tmp18)
            {
                var_x_4[var_i_6] = var_i_6;
                int32_t __tmp19 = 1;
                int32_t __tmp20 = var_i_6 + __tmp19;
                var_i_6 = __tmp20;
                goto L15;
            }
        }
    }
    char* __tmp21 = "reading values\n";
    int32_t __tmp22 = printf(__tmp21);
    {
        int32_t __tmp23 = 0;
        int32_t var_i_7 = __tmp23;
L24:
        {
            int32_t __tmp25 = 1;
            int32_t __tmp26 = var_n_3 - __tmp25;
            int __tmp27 = var_i_7 <= __tmp26;
            if (__tmp27)
            {
                char* __tmp28 = "%d\n";
                int32_t __tmp29 = printf(__tmp28, var_x_4[var_i_7]);
                int32_t __tmp30 = 1;
                int32_t __tmp31 = var_i_7 + __tmp30;
                var_i_7 = __tmp31;
                goto L24;
            }
        }
    }
}
int main(int argc, char** argv)
{
    lll_Main();
    int32_t __tmp32 = 0;
    return __tmp32;
}

