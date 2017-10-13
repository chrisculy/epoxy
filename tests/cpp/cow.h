#include <iostream>

#include "animal.h"

namespace farm
{
	class cow : public animal
	{
	public:
		void speak() const override
		{
			std::cout << "Moo!\n";
		}
	};
}
