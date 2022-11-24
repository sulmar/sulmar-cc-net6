import React from "react";
import axios from "axios";

export default async function Users() {
  const response = await axios.get("https://localhost:5001/api/users");

  console.log(response.data);
  const users = response.data;

  return (
    <>
      <ul>
        {users?.map((person) => (
          <li>Item</li>
          // <li key={person.Id}>
          //   {person.FirstName} {person.LastName}
          // </li>
        ))}

        <li>Item</li>
      </ul>
    </>
  );
}
