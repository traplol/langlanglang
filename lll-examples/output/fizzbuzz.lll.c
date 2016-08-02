#include <stddef.h>
#include <stdio.h>
#include <stdint.h>

void lll_Main(void);
char* func_entry_3(void);
int main(int argc, char** argv);


void lll_Main(void)
{
    char* __tmp5 = func_entry_3();
}
char* func_entry_3(void)
{
    {
        int32_t __tmp6 = 1;
        int32_t var_i_4 = __tmp6;
L7:
        {
            int32_t __tmp8 = 100;
            int __tmp9 = var_i_4 <= __tmp8;
            if (__tmp9)
            {
                {
                    int32_t __tmp10 = 15;
                    int32_t __tmp11 = var_i_4 % __tmp10;
                    int32_t __tmp12 = 0;
                    int __tmp13 = __tmp11 == __tmp12;
                    if (__tmp13)
                    {
                        char* __tmp14 = "fizzbuzz\n";
                        int32_t __tmp15 = printf(__tmp14);
                    }
                    else
                    {
                        {
                            int32_t __tmp16 = 5;
                            int32_t __tmp17 = var_i_4 % __tmp16;
                            int32_t __tmp18 = 0;
                            int __tmp19 = __tmp17 == __tmp18;
                            if (__tmp19)
                            {
                                char* __tmp20 = "fizz\n";
                                int32_t __tmp21 = printf(__tmp20);
                            }
                            else
                            {
                                {
                                    int32_t __tmp22 = 3;
                                    int32_t __tmp23 = var_i_4 % __tmp22;
                                    int32_t __tmp24 = 0;
                                    int __tmp25 = __tmp23 == __tmp24;
                                    if (__tmp25)
                                    {
                                        char* __tmp26 = "buzz\n";
                                        int32_t __tmp27 = printf(__tmp26);
                                    }
                                    else
                                    {
                                        char* __tmp28 = "%d\n";
                                        int32_t __tmp29 = printf(__tmp28, var_i_4);
                                    }
                                }
                            }
                        }
                    }
                }
                int32_t __tmp30 = 1;
                int32_t __tmp31 = var_i_4 + __tmp30;
                var_i_4 = __tmp31;
                goto L7;
            }
        }
    }
    char* __tmp32 = "horray";
    return __tmp32;
}
int main(int argc, char** argv)
{
    lll_Main();
    int32_t __tmp33 = 0;
    return __tmp33;
}

