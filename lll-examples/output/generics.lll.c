#include <stddef.h>
#include <stdio.h>
#include <stdint.h>
struct struc_Pet_0;
struct struc_Person_1;

void lll_Main(void);
void ext_Pet_Ctor_5(struct struc_Pet_0* var_this_6, char* var_type_7, char* var_name_8);
void ext_Person_Ctor_9(struct struc_Person_1* var_this_10, char* var_name_11, struct struc_Pet_0* var_pet_12);
void ext_Person_Dtor_14(struct struc_Person_1* var_this_15);
struct struc_Pet_0* ext_Person_GenericFunc_23(struct struc_Person_1* var_this_24, struct struc_Pet_0* var_par_25);
int16_t ext_Person_GenericFunc_27(struct struc_Person_1* var_this_28, int32_t var_par_29);
void func_PrintName_31(struct struc_Person_1* var_thing_32);
void func_PrintName_33(struct struc_Pet_0* var_thing_34);
int main(int argc, char** argv);

struct struc_Pet_0
{
    char* Type;
    char* Name;
};
struct struc_Person_1
{
    char* Name;
    struct struc_Pet_0* Pet;
};

void lll_Main(void)
{
    uint64_t __tmp35 = sizeof(struct struc_Pet_0);
    void* __tmp36 = malloc(__tmp35);
    struct struc_Pet_0* var___tmp18_19 = __tmp36;
    char* __tmp37 = "Cat";
    char* __tmp38 = "Marney";
    ext_Pet_Ctor_5(var___tmp18_19, __tmp37, __tmp38);
    struct struc_Pet_0* var_marney_17 = var___tmp18_19;
    uint64_t __tmp39 = sizeof(struct struc_Person_1);
    void* __tmp40 = malloc(__tmp39);
    struct struc_Person_1* var___tmp21_22 = __tmp40;
    char* __tmp41 = "Max";
    ext_Person_Ctor_9(var___tmp21_22, __tmp41, var_marney_17);
    struct struc_Person_1* var_max_20 = var___tmp21_22;
    struct struc_Pet_0* __tmp42 = ext_Person_GenericFunc_23(var_max_20, var_marney_17);
    struct struc_Pet_0* var_x_26 = __tmp42;
    int32_t __tmp43 = 255;
    int16_t __tmp44 = ext_Person_GenericFunc_27(var_max_20, __tmp43);
    int16_t var_y_30 = __tmp44;
    char* __tmp45 = "%x, %x\n";
    int32_t __tmp46 = printf(__tmp45, var_x_26, var_y_30);
    func_PrintName_31(var_max_20);
    func_PrintName_33(var_max_20->Pet);
    ext_Person_Dtor_14(var_max_20);
    free(var_max_20);
}
void ext_Pet_Ctor_5(struct struc_Pet_0* var_this_6, char* var_type_7, char* var_name_8)
{
    var_this_6->Type = var_type_7;
    var_this_6->Name = var_name_8;
}
void ext_Person_Ctor_9(struct struc_Person_1* var_this_10, char* var_name_11, struct struc_Pet_0* var_pet_12)
{
    var_this_10->Name = var_name_11;
    var_this_10->Pet = var_pet_12;
}
void ext_Person_Dtor_14(struct struc_Person_1* var_this_15)
{
    free(var_this_15->Pet);
}
struct struc_Pet_0* ext_Person_GenericFunc_23(struct struc_Person_1* var_this_24, struct struc_Pet_0* var_par_25)
{
    char* __tmp47 = "%s: %08x\n";
    int32_t __tmp48 = printf(__tmp47, var_this_24->Name, var_par_25);
    return var_par_25;
}
int16_t ext_Person_GenericFunc_27(struct struc_Person_1* var_this_28, int32_t var_par_29)
{
    char* __tmp49 = "%s: %08x\n";
    int32_t __tmp50 = printf(__tmp49, var_this_28->Name, var_par_29);
    return var_par_29;
}
void func_PrintName_31(struct struc_Person_1* var_thing_32)
{
    char* __tmp51 = "%s\n";
    int32_t __tmp52 = printf(__tmp51, var_thing_32->Name);
}
void func_PrintName_33(struct struc_Pet_0* var_thing_34)
{
    char* __tmp53 = "%s\n";
    int32_t __tmp54 = printf(__tmp53, var_thing_34->Name);
}
int main(int argc, char** argv)
{
    lll_Main();
    int32_t __tmp55 = 0;
    return __tmp55;
}

