#include <stddef.h>
#include <stdio.h>
#include <stdint.h>
struct struc_StructWithFixedArray_0;

void lll_Main(void);
void ext_StructWithFixedArray_Ctor_4(struct struc_StructWithFixedArray_0* var_this_5);
void ext_StructWithFixedArray_fill_6(struct struc_StructWithFixedArray_0* var_this_7);
void ext_StructWithFixedArray_zero_8(struct struc_StructWithFixedArray_0* var_this_9);
void ext_StructWithFixedArray_print_10(struct struc_StructWithFixedArray_0* var_this_11);
int main(int argc, char** argv);

struct struc_StructWithFixedArray_0
{
    int32_t arr[20];
};

void lll_Main(void)
{
    uint64_t __tmp18 = sizeof(struct struc_StructWithFixedArray_0);
    void* __tmp19 = malloc(__tmp18);
    struct struc_StructWithFixedArray_0* var___tmp16_17 = __tmp19;
    ext_StructWithFixedArray_Ctor_4(var___tmp16_17);
    struct struc_StructWithFixedArray_0* var_s_15 = var___tmp16_17;
    ext_StructWithFixedArray_print_10(var_s_15);
    ext_StructWithFixedArray_zero_8(var_s_15);
    ext_StructWithFixedArray_print_10(var_s_15);
    ext_StructWithFixedArray_fill_6(var_s_15);
    ext_StructWithFixedArray_print_10(var_s_15);
}
void ext_StructWithFixedArray_Ctor_4(struct struc_StructWithFixedArray_0* var_this_5)
{
    ext_StructWithFixedArray_fill_6(var_this_5);
}
void ext_StructWithFixedArray_fill_6(struct struc_StructWithFixedArray_0* var_this_7)
{
    {
        int32_t __tmp20 = 0;
        int32_t var_i_12 = __tmp20;
L21:
        {
            int32_t __tmp22 = 19;
            int __tmp23 = var_i_12 <= __tmp22;
            if (__tmp23)
            {
                int32_t __tmp24 = 1;
                int32_t __tmp25 = var_i_12 + __tmp24;
                var_this_7->arr[var_i_12] = __tmp25;
                int32_t __tmp26 = 1;
                int32_t __tmp27 = var_i_12 + __tmp26;
                var_i_12 = __tmp27;
                goto L21;
            }
        }
    }
}
void ext_StructWithFixedArray_zero_8(struct struc_StructWithFixedArray_0* var_this_9)
{
    {
        int32_t __tmp28 = 0;
        int32_t var_i_13 = __tmp28;
L29:
        {
            int32_t __tmp30 = 19;
            int __tmp31 = var_i_13 <= __tmp30;
            if (__tmp31)
            {
                int32_t __tmp32 = 0;
                var_this_9->arr[var_i_13] = __tmp32;
                int32_t __tmp33 = 1;
                int32_t __tmp34 = var_i_13 + __tmp33;
                var_i_13 = __tmp34;
                goto L29;
            }
        }
    }
}
void ext_StructWithFixedArray_print_10(struct struc_StructWithFixedArray_0* var_this_11)
{
    {
        int32_t __tmp35 = 0;
        int32_t var_i_14 = __tmp35;
L36:
        {
            int32_t __tmp37 = 19;
            int __tmp38 = var_i_14 <= __tmp37;
            if (__tmp38)
            {
                char* __tmp39 = "%2d";
                int32_t __tmp40 = printf(__tmp39, var_this_11->arr[var_i_14]);
                {
                    int32_t __tmp41 = 1;
                    int32_t __tmp42 = var_i_14 + __tmp41;
                    int32_t __tmp43 = 19;
                    int __tmp44 = __tmp42 <= __tmp43;
                    if (__tmp44)
                    {
                        char* __tmp45 = ", ";
                        int32_t __tmp46 = printf(__tmp45);
                    }
                }
                int32_t __tmp47 = 1;
                int32_t __tmp48 = var_i_14 + __tmp47;
                var_i_14 = __tmp48;
                goto L36;
            }
        }
    }
    char* __tmp49 = "\n";
    int32_t __tmp50 = printf(__tmp49);
}
int main(int argc, char** argv)
{
    lll_Main();
    int32_t __tmp51 = 0;
    return __tmp51;
}

