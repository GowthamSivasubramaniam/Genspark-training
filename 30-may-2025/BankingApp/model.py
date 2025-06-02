from flask import Flask, request, jsonify
import pandas as pd
from sklearn.metrics.pairwise import cosine_similarity
import logging
from sentence_transformers import SentenceTransformer
import numpy as np
app = Flask(__name__)
df = pd.read_csv("BankFAQs.csv")
import warnings
warnings.filterwarnings("ignore")

model = SentenceTransformer('all-MiniLM-L6-v2') 
faq_questions = df['Question'].tolist()
faq_embeddings = model.encode(faq_questions, convert_to_tensor=True)

@app.route('/get_answer', methods=['POST'])
def get_most_similar_answer():
    
    data = request.json
    print(data)
    user_question = data['Question']
    user_embedding = model.encode([user_question], convert_to_tensor=True)
   
    similarities = cosine_similarity(user_embedding.cpu(), faq_embeddings.cpu())[0]
    best_idx = np.argmax(similarities)
    best_question = faq_questions[best_idx]
    best_answer = df.iloc[best_idx]['Answer']
    confidence = float(similarities[best_idx])  
    if(confidence< 0.40):
     return jsonify({'answer' : 'sorry! Kindly contact the customer care '})
    app.logger.info(f"Best matched question: {best_question} | Confidence: {confidence:.2f}")

    return jsonify({'answer': best_answer, 'confidence': confidence})


if __name__ == '__main__':
    app.run(port=6003, debug=True)
