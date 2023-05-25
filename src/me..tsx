import React, { useEffect, useState } from 'react';

export function Me() {
  const [me, setMe] = useState({ me: 'huh' });

  const fetchMe = () => {
    fetch('/.auth/me')
      .then((res) => res.json())
      .then((json) => setMe({ me: JSON.stringify(json, null, 2) }));
  };

  useEffect(() => {
    fetchMe();
  }, []);

  return (
    <div>
      <pre>{me.me}</pre>
      <a href="/.auth/logout">logout</a>
    </div>
  );
}

export default Me;
