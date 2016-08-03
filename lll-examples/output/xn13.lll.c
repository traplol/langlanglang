#include <stddef.h>
#include <stdio.h>
#include <stdint.h>

void lll_Main(void);
int main(int argc, char** argv);


void lll_Main(void)
{
    int32_t __tmp7 = 1;
    int64_t var_i_5 = __tmp7;
    {
L8:
        {
            int32_t __tmp9 = 0;
            int __tmp10 = var_i_5 > __tmp9;
            if (__tmp10)
            {
                {
                    int64_t var_j_6 = var_i_5;
                    int32_t __tmp11 = 2;
                    int64_t __tmp12 = __tmp11 * var_i_5;
                    int32_t __tmp13 = 2;
                    int64_t __tmp14 = var_i_5 / __tmp13;
                    int64_t __tmp15 = __tmp12 ^ __tmp14;
                    var_i_5 = __tmp15;
L16:
                    {
                        if (var_j_6)
                        {
                            int32_t __tmp17 = 1;
                            int64_t __tmp18 = var_j_6 & __tmp17;
                            char* __tmp19 = " X";
                            int32_t __tmp20 = putchar(__tmp19[__tmp18]);
                            int32_t __tmp21 = 2;
                            int64_t __tmp22 = var_j_6 / __tmp21;
                            var_j_6 = __tmp22;
                            goto L16;
                        }
                    }
                }
                char* __tmp23 = "";
                int32_t __tmp24 = puts(__tmp23);
                goto L8;
            }
        }
    }
}
int main(int argc, char** argv)
{
    lll_Main();
    int32_t __tmp25 = 0;
    return __tmp25;
}

