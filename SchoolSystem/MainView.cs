using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;   //引入命名空间
using MySql.Data.MySqlClient;

namespace SchoolSystem
{
    /// <summary>
    /// 主界面
    /// </summary>
    public class MainView
    {
        string connStr = "server=localhost;database=schoolsystemdb;uid=root;pwd=123456;charset=utf8";
        public void Login()
        {
            Console.WriteLine("欢迎登录");
            Console.WriteLine("请输入账号：");
            string? account = Console.ReadLine();
            Console.WriteLine("请输入密码：");
            string? password = Console.ReadLine();  
            //创建MySql数据库连接对象
            MySqlConnection conn = new MySqlConnection(connStr);//用ip地址需要加端口号3306 server=192.168.8.52,3306；server=localhost 也可以
            //打开数据库连接
            conn.Open();
            //创建执行脚本对象   系统存储过程方式：过滤敏感字符操作  参数化
            string sql = $"SELECT * FROM users WHERE account=@account AND `password` =@password";
            MySqlCommand command = new MySqlCommand(sql, conn);
            //防止sql注入（如输入万能密码 ' or 1=1#..）需要这个操作  
            MySqlParameter para1 = new MySqlParameter("@account",account);
            MySqlParameter para2 = new MySqlParameter("@password",password);
            command.Parameters.Add(para1);
            command.Parameters.Add(para2);
            //也可以用下面两行代码替换上述
            //MySqlParameter[] paras = { new MySqlParameter("@account", account), new MySqlParameter("@password", password };
            //command.Parameters.AddRange(paras);//addrange 添加数组
            //执行脚本 返回数据库游标对象
            MySqlDataReader reader =  command.ExecuteReader();
            if (reader.Read())
            {
                Console.WriteLine($"登录成功！账号:{reader["account"]},姓名:{reader["name"]}");
            }
            else
            {
                Console.WriteLine("登录不成功");
            }
            //释放资源
            reader.Close();
            conn.Close();

        }
        public void Register()
        {
            Console.WriteLine("注册界面");
            Console.WriteLine("请输入账号：");
            string? account = Console.ReadLine();
            Console.WriteLine("请输入密码：");
            string? password = Console.ReadLine();
            Console.WriteLine("请输入姓名：");
            string? name = Console.ReadLine();
            Console.WriteLine("请输入年龄：");
            int age = 0;
            int.TryParse(Console.ReadLine(), out age);
            //string? age = int.Parse(Console.ReadLine());
            //创建MySql数据库连接对象
            MySqlConnection conn = new MySqlConnection(connStr);//用ip地址需要加端口号3306 server=192.168.8.52,3306；server=localhost 也可以
            //打开数据库连接
            conn.Open();
            //创建执行脚本对象
            string sql = $@"INSERT INTO users(account,`password`,`name`,age)
                                                values('{account}','{password}','{name}',{age})";
            
            MySqlCommand command = new MySqlCommand(sql, conn);
            //执行脚本 
            int result = command.ExecuteNonQuery();//返回受影响行数
            if(result > 0)
            {
                Console.WriteLine("添加成功");
            }
            else
            {
                Console.WriteLine("添加失败");
            }
            //释放资源
            conn.Close();
        }
    }
}
