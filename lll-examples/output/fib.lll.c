#include <stddef.h>
#include <stdio.h>
#include <stdint.h>

void lll_Main(void);
int64_t func_fib_3(int64_t var_n_4);
int main(int argc, char** argv);


void lll_Main(void)
{
    int32_t __tmp7 = 40;
    int8_t var_n_5 = __tmp7;
    char* __tmp8 = "first %d fibonacci numbers!\n";
    int32_t __tmp9 = printf(__tmp8, var_n_5);
    {
        int32_t __tmp10 = 0;
        int8_t var_i_6 = __tmp10;
L11:
        {
            int32_t __tmp12 = 40;
            int __tmp13 = var_i_6 <= __tmp12;
            if (__tmp13)
            {
                char* __tmp14 = "%d: %lld\n";
                int64_t __tmp15 = func_fib_3(var_i_6);
                int32_t __tmp16 = printf(__tmp14, var_i_6, __tmp15);
                int32_t __tmp17 = 1;
                int32_t __tmp18 = var_i_6 + __tmp17;
                var_i_6 = __tmp18;
                goto L11;
            }
        }
    }
}
int64_t func_fib_3(int64_t var_n_4)
{
    {
        int32_t __tmp19 = 2;
        int __tmp20 = var_n_4 < __tmp19;
        if (__tmp20)
        {
            return var_n_4;
        }
    }
    int32_t __tmp21 = 1;
    int64_t __tmp22 = var_n_4 - __tmp21;
    int64_t __tmp23 = func_fib_3(__tmp22);
    int32_t __tmp24 = 2;
    int64_t __tmp25 = var_n_4 - __tmp24;
    int64_t __tmp26 = func_fib_3(__tmp25);
    int64_t __tmp27 = __tmp23 + __tmp26;
    return __tmp27;
}
int main(int argc, char** argv)
{
    lll_Main();
    int32_t __tmp28 = 0;
    return __tmp28;
}

