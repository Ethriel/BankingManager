import React from 'react';
import PropTypes from 'prop-types';

const LinkButton = ({ to, children, external, className, style, accountNumber, ...props }) => {
  return (
    <a
      href={to}
      className={className}
      style={{
        display: 'inline-block',
        textDecoration: 'none',
        padding: '10px 20px',
        backgroundColor: '#007bff',
        color: '#fff',
        borderRadius: '5px',
        textAlign: 'center',
        cursor: 'pointer',
        ...style,
      }}
      target={external ? '_blank' : '_self'}
      rel={external ? 'noopener noreferrer' : undefined}
      {...props}
    >
      {children}
    </a>
  );
};

LinkButton.propTypes = {
  to: PropTypes.string.isRequired,
  children: PropTypes.node.isRequired,
  external: PropTypes.bool,
  className: PropTypes.string,
  style: PropTypes.object,
};

LinkButton.defaultProps = {
  external: false,
  className: '',
  style: {},
};

export default LinkButton;