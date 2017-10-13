#include <string>

namespace farm
{
namespace statistics
{
	class data
	{
	public:
		void* _voidPointer = reinterpret_cast<void*>(0x04);
		const void* _constVoidPointer = reinterpret_cast<const void*>(0x03);

		bool _bool = true;
		const bool _constBool = true;

		char _char = 'a';
		const char _constChar = 'b';
		char16_t _char16 = u'k';
		const char16_t _constChar16 = u'l';
		char32_t _char32 = U's';
		const char32_t _constChar32 = U't';

		int8_t _int8 = -128;
		const int8_t _constInt8 = 127;
		int16_t _int16 = -32768;
		const int16_t _constInt16 = 32767;
		int32_t _int32 = -2147483648;
		const int32_t _constInt32 = 2147483647;

		uint8_t _uint8 = 254;
		const uint8_t _constUInt8 = 255;
		uint16_t _uint16 = 65534;
		const uint16_t _constUInt16 = 65535;
		uint32_t _uint32 = 4294967294;
		const uint32_t _constUInt32 = 4294967295;

		float _float = 12.37f;
		const float _constFloat = 123456.789f;
		double _double = 42366.2342;
		const double _constDouble = 9442945.20435;

		std::string _string = "Hi";
		const std::string _constString = "Hello";
	};
}
}
