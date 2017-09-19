#include <iostream>
#include <memory>
#include <string>

namespace test
{
namespace api
{
    enum test_enum
    {
        RED,
        YELLOW,
        GREEN,
        BLUE
    };

    enum class test_enum_class
    {
        START,
        STOP,
        PAUSE,
        REWIND,
        FAST_FORWARD
    };

    struct test_struct
    {
        test_struct() = default;

        test_struct(int x, int y)
            : x{ x }
            , y{ y }
        {
        }

        int x = 4;
        int y = 7;
    };

    class test_class
    {
    public:
        test_class() = default;

        test_class(std::string testString)
            : m_string{ testString }
        {
        }

        test_class(std::string testString, int testInt)
            : m_string{ testString }
            , m_int32{ testInt }
        {
        }

        void function_void()
        {
            std::cout << "test::api::test_class:function_void\n";
        }

        bool function_bool() const
        {
            std::cout << "test::api::test_class:function_bool\n";
            return m_constBool;
        }

        std::string function_string()
        {
            std::cout << "test::api::test_class:function_string\n";
            return m_constString;
        }

        int function_int(const std::string & stringParam, int16_t int16Param)
        {
            std::cout << "test::api::test_class:function_int: '" << stringParam << "', " << int16Param << "\n";
            return 32;
        }

        void* m_voidPointer = reinterpret_cast<void*>(0x04);
        const void* m_constVoidPointer = reinterpret_cast<const void*>(0x03);

        bool m_bool = true;
        const bool m_constBool = true;

        char m_char = 'a';
        const char m_constChar = 'b';
        char16_t m_char16 = u'k';
        const char16_t m_constChar16 = u'l';
        char32_t m_char32 = U's';
        const char32_t m_constChar32 = U't';

        int8_t m_int8 = -128;
        const int8_t m_constInt8 = 127;
        int16_t m_int16 = -32768;
        const int16_t m_constInt16 = 32767;
        int32_t m_int32 = -2147483648;
        const int32_t m_constInt32 = 2147483647;

        uint8_t m_uint8 = 254;
        const uint8_t m_constUInt8 = 255;
        uint16_t m_uint16 = 65534;
        const uint16_t m_constUInt16 = 65535;
        uint32_t m_uint32 = 4294967294;
        const uint32_t m_constUInt32 = 4294967295;

        float m_float = 12.37f;
        const float m_constFloat = 123456.789f;
        double m_double = 42366.2342;
        const double m_constDouble = 9442945.20435;
 
        std::string m_string = "Hi";
        const std::string m_constString = "Hello";

        test_enum m_enum = RED;
        const test_enum m_constEnum = BLUE;
        test_enum_class m_enumClass = test_enum_class::START;
        const test_enum_class m_constEnumClass = test_enum_class::FAST_FORWARD;

        test_struct m_struct;
        const test_struct m_constStruct = test_struct{ 3, 9 };
    };
}
}
