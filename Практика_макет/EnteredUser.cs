using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Практика_макет
{
    public class EnteredUser
    {
        private static EnteredUser _instance;
        private User _currentUser;
        private int _currentType;
        // Приватный конструктор для предотвращения создания экземпляров снаружи класса
        private EnteredUser() { }

        // Свойство для доступа к единственному экземпляру синглтона
        public static EnteredUser Instance
        {
            get
            {
                // Если экземпляр еще не создан, создаем его
                if (_instance == null)
                {
                    _instance = new EnteredUser();
                }
                return _instance;
            }
        }

        // Свойство для доступа к текущему пользователю
        public User CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser = value; }
        }


        public int CurrentType
        {
            get { return _currentType; }
            set { _currentType = value; }
        }

        // Метод для очистки данных пользователя при выходе из системы
        public void ClearSession()
        {
            _currentUser = null;
        }
    }

}
