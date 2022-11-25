import React from "react";
import { useState, useEffect } from "react";
import axios from "axios";

export default function Users() {
  const [users, setUsers] = useState([]);

  const getUsers = async () => {
    try {
      const response = await axios.get("https://localhost:5001/api/users");
      const users = response.data;
      setUsers(users);
    } catch (err) {
      console.log("error occured when fetching users");
    }
  };

  useEffect(() => {
    // async function getUsers() {
    //   try {
    //     const response = await axios.get("https://localhost:5001/api/users");
    //     const users = response.data;
    //     setUsers(users);
    //   } catch (err) {
    //     console.log("error occured when fetching users");
    //   }
    // }
    console.log("get users");
    getUsers();
  }, []);

  return (
    <>
      <ul>
        {users.map((person) => (
          <li key={person.id}>
            {person.firstName} {person.lastName}
          </li>
        ))}
      </ul>
    </>
  );
}
