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
            int __tmp12 = var_i_6 <= var_n_5;
            if (__tmp12)
            {
                char* __tmp13 = "%d: %lld\n";
                int64_t __tmp14 = func_fib_3(var_i_6);
                int32_t __tmp15 = printf(__tmp13, var_i_6, __tmp14);
                int32_t __tmp16 = 1;
                int32_t __tmp17 = var_i_6 + __tmp16;
                var_i_6 = __tmp17;
                goto L11;
            }
        }
    }
}
int64_t func_fib_3(int64_t var_n_4)
{
    {
        int32_t __tmp18 = 2;
        int __tmp19 = var_n_4 < __tmp18;
        if (__tmp19)
        {
            return var_n_4;
        }
    }
    int32_t __tmp20 = 1;
    int64_t __tmp21 = var_n_4 - __tmp20;
    int64_t __tmp22 = func_fib_3(__tmp21);
    int32_t __tmp23 = 2;
    int64_t __tmp24 = var_n_4 - __tmp23;
    int64_t __tmp25 = func_fib_3(__tmp24);
    int64_t __tmp26 = __tmp22 + __tmp25;
    return __tmp26;
}
int main(int argc, char** argv)
{
    lll_Main();
    int32_t __tmp27 = 0;
    return __tmp27;
}

