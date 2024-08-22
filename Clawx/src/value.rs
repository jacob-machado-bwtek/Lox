use std::fmt;


#[derive(Clone)]
pub enum ValueType{
    Number(f64),
}

#[derive(Clone)]
pub struct Value{
     value_type: ValueType,
}

impl Value{
    pub fn number_val(val: f64) -> Value{
        Value{
            value_type: ValueType::Number(val)
        }
    }
    pub fn as_number(val: Value) -> f64{
        match val.value_type{
            ValueType::Number(val) => val,
            _ => panic!("Value::as_number should never be called on a non f64 type"),
        }
    }
    pub fn is_number(val: &Value) -> bool{
        match val.value_type {
            ValueType::Number(_) => true,
            _ => false
        }
    }
}

impl fmt::Display for Value{
    fn fmt(&self, f: &mut fmt::Formatter<'_>) -> fmt::Result{
        let val = self.value_type.clone();
        match val{
            ValueType::Number(v) => write!(f, "{}",v),
        }
    }
}