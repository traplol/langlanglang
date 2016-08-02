#include <stddef.h>
#include <stdio.h>
#include <stdint.h>
struct struc_Vector_0;

void lll_Main(void);
void ext_Vector_Ctor_4(struct struc_Vector_0* var_this_5);
void ext_Vector_Dtor_6(struct struc_Vector_0* var_this_7);
void ext_Vector_push_back_8(struct struc_Vector_0* var_this_9, int32_t var_item_10);
struct struc_Vector_0* ext_Vector___lshift___11(struct struc_Vector_0* var_this_12, int32_t var_item_13);
int32_t ext_Vector_pop_back_14(struct struc_Vector_0* var_this_15);
void ext_Vector_resize_16(struct struc_Vector_0* var_this_17, uint64_t var_newSize_18);
void ext_Vector_print_elems_19(struct struc_Vector_0* var_this_20);
uint64_t func_min_24(uint64_t var_x_25, uint64_t var_y_26);
int main(int argc, char** argv);

struct struc_Vector_0
{
    uint64_t Capacity;
    uint64_t Length;
    int32_t* Array;
};

void lll_Main(void)
{
    uint64_t __tmp34 = sizeof(struct struc_Vector_0);
    void* __tmp35 = malloc(__tmp34);
    struct struc_Vector_0* var___tmp32_33 = __tmp35;
    ext_Vector_Ctor_4(var___tmp32_33);
    struct struc_Vector_0* var_test_31 = var___tmp32_33;
    int32_t __tmp36 = 1;
    struct struc_Vector_0* __tmp37 = ext_Vector___lshift___11(var_test_31, __tmp36);
    int32_t __tmp38 = 2;
    struct struc_Vector_0* __tmp39 = ext_Vector___lshift___11(__tmp37, __tmp38);
    int32_t __tmp40 = 3;
    struct struc_Vector_0* __tmp41 = ext_Vector___lshift___11(__tmp39, __tmp40);
    int32_t __tmp42 = 4;
    struct struc_Vector_0* __tmp43 = ext_Vector___lshift___11(__tmp41, __tmp42);
    int32_t __tmp44 = 5;
    struct struc_Vector_0* __tmp45 = ext_Vector___lshift___11(__tmp43, __tmp44);
    ext_Vector_print_elems_19(var_test_31);
}
void ext_Vector_Ctor_4(struct struc_Vector_0* var_this_5)
{
    int32_t __tmp46 = 8;
    var_this_5->Capacity = __tmp46;
    int32_t __tmp47 = 0;
    var_this_5->Length = __tmp47;
    uint64_t __tmp48 = sizeof(int32_t);
    uint64_t __tmp49 = var_this_5->Capacity * __tmp48;
    void* __tmp50 = malloc(__tmp49);
    var_this_5->Array = __tmp50;
}
void ext_Vector_Dtor_6(struct struc_Vector_0* var_this_7)
{
    int32_t __tmp51 = 0;
    var_this_7->Capacity = __tmp51;
    int32_t __tmp52 = 0;
    var_this_7->Length = __tmp52;
    free(var_this_7->Array);
}
void ext_Vector_push_back_8(struct struc_Vector_0* var_this_9, int32_t var_item_10)
{
    {
        int32_t __tmp53 = 1;
        int64_t __tmp54 = var_this_9->Length + __tmp53;
        int __tmp55 = __tmp54 >= var_this_9->Capacity;
        if (__tmp55)
        {
            int32_t __tmp56 = 2;
            int64_t __tmp57 = var_this_9->Capacity * __tmp56;
            ext_Vector_resize_16(var_this_9, __tmp57);
        }
    }
    var_this_9->Array[var_this_9->Length] = var_item_10;
    int32_t __tmp58 = 1;
    int64_t __tmp59 = var_this_9->Length + __tmp58;
    var_this_9->Length = __tmp59;
}
struct struc_Vector_0* ext_Vector___lshift___11(struct struc_Vector_0* var_this_12, int32_t var_item_13)
{
    ext_Vector_push_back_8(var_this_12, var_item_13);
    return var_this_12;
}
int32_t ext_Vector_pop_back_14(struct struc_Vector_0* var_this_15)
{
    int32_t __tmp60 = 1;
    int64_t __tmp61 = var_this_15->Length - __tmp60;
    int32_t var_tmp_22 = var_this_15->Array[__tmp61];
    int32_t __tmp62 = 0;
    int32_t __tmp63 = 1;
    int64_t __tmp64 = var_this_15->Length - __tmp63;
    var_this_15->Array[__tmp64] = __tmp62;
    int32_t __tmp65 = 1;
    int64_t __tmp66 = var_this_15->Length - __tmp65;
    var_this_15->Length = __tmp66;
    return var_tmp_22;
}
void ext_Vector_resize_16(struct struc_Vector_0* var_this_17, uint64_t var_newSize_18)
{
    uint64_t __tmp67 = sizeof(int32_t);
    uint64_t __tmp68 = var_newSize_18 * __tmp67;
    void* __tmp69 = malloc(__tmp68);
    int32_t* var_newArray_23 = __tmp69;
    uint64_t __tmp70 = func_min_24(var_this_17->Length, var_newSize_18);
    uint64_t var_limit_27 = __tmp70;
    {
        int32_t __tmp71 = 0;
        int8_t var_i_28 = __tmp71;
L72:
        {
            int __tmp73 = var_i_28 < var_limit_27;
            if (__tmp73)
            {
                var_newArray_23[var_i_28] = var_this_17->Array[var_i_28];
                int32_t __tmp74 = 1;
                int32_t __tmp75 = var_i_28 + __tmp74;
                var_i_28 = __tmp75;
                goto L72;
            }
        }
    }
    int32_t* var_tmp_29 = var_this_17->Array;
    var_this_17->Array = var_newArray_23;
    free(var_tmp_29);
}
void ext_Vector_print_elems_19(struct struc_Vector_0* var_this_20)
{
    char* __tmp76 = "E: ";
    int32_t __tmp77 = printf(__tmp76);
    {
        int32_t __tmp78 = 0;
        int8_t var_itr_30 = __tmp78;
L79:
        {
            int __tmp80 = var_itr_30 < var_this_20->Length;
            if (__tmp80)
            {
                char* __tmp81 = "%d";
                int32_t __tmp82 = printf(__tmp81, var_this_20->Array[var_itr_30]);
                {
                    int32_t __tmp83 = 1;
                    int32_t __tmp84 = var_itr_30 + __tmp83;
                    int __tmp85 = __tmp84 < var_this_20->Length;
                    if (__tmp85)
                    {
                        char* __tmp86 = ", ";
                        int32_t __tmp87 = printf(__tmp86);
                    }
                }
                int32_t __tmp88 = 1;
                int32_t __tmp89 = var_itr_30 + __tmp88;
                var_itr_30 = __tmp89;
                goto L79;
            }
        }
    }
    char* __tmp90 = "\n";
    int32_t __tmp91 = printf(__tmp90);
}
uint64_t func_min_24(uint64_t var_x_25, uint64_t var_y_26)
{
    {
        int __tmp92 = var_x_25 < var_y_26;
        if (__tmp92)
        {
            return var_x_25;
        }
    }
    return var_y_26;
}
int main(int argc, char** argv)
{
    lll_Main();
    int32_t __tmp93 = 0;
    return __tmp93;
}

