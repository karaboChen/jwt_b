import{u,a as r,o as l,c as i,b as t,t as _,d as h,F as m,G as p}from"./index-e2c8282b.js";const g={class:"about"},b=t("h1",null,"This is an about page",-1),C={__name:"AboutView",setup(d){const e=u(),n=r();async function a(){try{let o=await p();console.log(o.data)}catch(o){console.log(o)}}function s(){n.push({name:"home"})}function c(){e.increment()}return(o,f)=>(l(),i(m,null,[t("div",g,[t("h2",null,_(h(e).count),1),t("button",{onClick:c},"+++++"),b]),t("button",{onClick:a},"登入"),t("button",{onClick:s},"跳")],64))}};export{C as default};